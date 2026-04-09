using System;
using System.Collections.Generic;
using LegacyRenewalApp.Calculators;
using LegacyRenewalApp.Calculators.Policies;
using LegacyRenewalApp.Gateways;
using LegacyRenewalApp.Repositories;

namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISubscriptionPlanRepository _planRepository;
        private readonly IDiscountCalculator _discountCalculator;
        private readonly ISupportFeeCalculator _supportFeeCalculator;
        private readonly IPaymentFeeCalculator _paymentFeeCalculator;
        private readonly ITaxCalculator _taxCalculator;
        private readonly IBillingGateway _billingGateway;

        public SubscriptionRenewalService() : this(
            new CustomerRepository(),
            new SubscriptionPlanRepository(),
            new DiscountCalculator(new List<IDiscountPolicy> 
            { 
                new SegmentDiscountPolicy(),
                new TenureDiscountPolicy(),
                new VolumeDiscountPolicy(),
                new LoyaltyPointsDiscountPolicy()
            }),
            new SupportFeeCalculator(),
            new PaymentFeeCalculator(new List<IPaymentFeePolicy> 
            { 
                new CardPaymentFeePolicy(),
                new BankTransferPaymentFeePolicy(),
                new PaypalPaymentFeePolicy(),
                new InvoicePaymentFeePolicy()
            }),
            new TaxCalculator(),
            new LegacyBillingGatewayAdapter()
        )
        {
        }

        public SubscriptionRenewalService(
            ICustomerRepository customerRepository,
            ISubscriptionPlanRepository planRepository,
            IDiscountCalculator discountCalculator,
            ISupportFeeCalculator supportFeeCalculator,
            IPaymentFeeCalculator paymentFeeCalculator,
            ITaxCalculator taxCalculator,
            IBillingGateway billingGateway)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _planRepository = planRepository ?? throw new ArgumentNullException(nameof(planRepository));
            _discountCalculator = discountCalculator ?? throw new ArgumentNullException(nameof(discountCalculator));
            _supportFeeCalculator = supportFeeCalculator ?? throw new ArgumentNullException(nameof(supportFeeCalculator));
            _paymentFeeCalculator = paymentFeeCalculator ?? throw new ArgumentNullException(nameof(paymentFeeCalculator));
            _taxCalculator = taxCalculator ?? throw new ArgumentNullException(nameof(taxCalculator));
            _billingGateway = billingGateway ?? throw new ArgumentNullException(nameof(billingGateway));
        }

        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {
            if (customerId <= 0)
                throw new ArgumentException("Customer id must be positive");
            if (string.IsNullOrWhiteSpace(planCode))
                throw new ArgumentException("Plan code is required");
            if (seatCount <= 0)
                throw new ArgumentException("Seat count must be positive");
            if (string.IsNullOrWhiteSpace(paymentMethod))
                throw new ArgumentException("Payment method is required");

            string normalizedPlanCode = planCode.Trim().ToUpperInvariant();
            string normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();

            var customer = _customerRepository.GetById(customerId);
            var plan = _planRepository.GetByCode(normalizedPlanCode);

            if (!customer.IsActive)
                throw new InvalidOperationException("Inactive customers cannot renew subscriptions");

            decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;
            
            var discountResult = _discountCalculator.CalculateTotalDiscount(customer, plan, seatCount, baseAmount, useLoyaltyPoints);
            decimal subtotalAfterDiscount = baseAmount - discountResult.Amount;
            
            string notes = discountResult.Notes;

            if (subtotalAfterDiscount < 300m)
            {
                subtotalAfterDiscount = 300m;
                notes += "minimum discounted subtotal applied; ";
            }

            var supportFeeResult = _supportFeeCalculator.CalculateSupportFee(normalizedPlanCode, includePremiumSupport);
            decimal supportFee = supportFeeResult.Amount;
            notes += supportFeeResult.Notes;

            decimal amountToFee = subtotalAfterDiscount + supportFee;
            var paymentFeeResult = _paymentFeeCalculator.CalculatePaymentFee(normalizedPaymentMethod, amountToFee);
            decimal paymentFee = paymentFeeResult.Amount;
            notes += paymentFeeResult.Notes;

            decimal taxRate = _taxCalculator.CalculateTaxRate(customer.Country);
            decimal taxBase = subtotalAfterDiscount + supportFee + paymentFee;
            decimal taxAmount = taxBase * taxRate;
            decimal finalAmount = taxBase + taxAmount;

            if (finalAmount < 500m)
            {
                finalAmount = 500m;
                notes += "minimum invoice amount applied; ";
            }

            var invoice = new RenewalInvoice
            {
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{customerId}-{normalizedPlanCode}",
                CustomerName = customer.FullName,
                PlanCode = normalizedPlanCode,
                PaymentMethod = normalizedPaymentMethod,
                SeatCount = seatCount,
                BaseAmount = Math.Round(baseAmount, 2, MidpointRounding.AwayFromZero),
                DiscountAmount = Math.Round(discountResult.Amount, 2, MidpointRounding.AwayFromZero),
                SupportFee = Math.Round(supportFee, 2, MidpointRounding.AwayFromZero),
                PaymentFee = Math.Round(paymentFee, 2, MidpointRounding.AwayFromZero),
                TaxAmount = Math.Round(taxAmount, 2, MidpointRounding.AwayFromZero),
                FinalAmount = Math.Round(finalAmount, 2, MidpointRounding.AwayFromZero),
                Notes = notes.Trim(),
                GeneratedAt = DateTime.UtcNow
            };

            _billingGateway.SaveInvoice(invoice);

            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                string subject = "Subscription renewal invoice";
                string body = $"Hello {customer.FullName}, your renewal for plan {normalizedPlanCode} has been prepared. Final amount: {invoice.FinalAmount:F2}.";
                _billingGateway.SendEmail(customer.Email, subject, body);
            }

            return invoice;
        }
    }
}
