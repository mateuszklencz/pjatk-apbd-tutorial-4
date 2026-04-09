using System;
using System.Collections.Generic;
using System.Linq;
using LegacyRenewalApp.Calculators.Policies;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators
{
    public class PaymentFeeCalculator : IPaymentFeeCalculator
    {
        private readonly IEnumerable<IPaymentFeePolicy> _policies;

        public PaymentFeeCalculator(IEnumerable<IPaymentFeePolicy> policies)
        {
            _policies = policies ?? throw new ArgumentNullException(nameof(policies));
        }

        public CalculationResult CalculatePaymentFee(string paymentMethod, decimal amountToFee)
        {
            var policy = _policies.FirstOrDefault(p => p.IsApplicable(paymentMethod));
            if (policy == null)
            {
                throw new ArgumentException("Unsupported payment method");
            }

            return policy.CalculateFee(amountToFee);
        }
    }
}
