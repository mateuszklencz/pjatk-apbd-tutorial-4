using System;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators.Policies
{
    public class CardPaymentFeePolicy : IPaymentFeePolicy
    {
        public bool IsApplicable(string paymentMethod) => paymentMethod == "CARD";
        
        public CalculationResult CalculateFee(decimal amountToFee)
        {
            return new CalculationResult { Amount = amountToFee * 0.02m, Notes = "card payment fee; " };
        }
    }

    public class BankTransferPaymentFeePolicy : IPaymentFeePolicy
    {
        public bool IsApplicable(string paymentMethod) => paymentMethod == "BANK_TRANSFER";
        
        public CalculationResult CalculateFee(decimal amountToFee)
        {
            return new CalculationResult { Amount = amountToFee * 0.01m, Notes = "bank transfer fee; " };
        }
    }

    public class PaypalPaymentFeePolicy : IPaymentFeePolicy
    {
        public bool IsApplicable(string paymentMethod) => paymentMethod == "PAYPAL";
        
        public CalculationResult CalculateFee(decimal amountToFee)
        {
            return new CalculationResult { Amount = amountToFee * 0.035m, Notes = "paypal fee; " };
        }
    }

    public class InvoicePaymentFeePolicy : IPaymentFeePolicy
    {
        public bool IsApplicable(string paymentMethod) => paymentMethod == "INVOICE";
        
        public CalculationResult CalculateFee(decimal amountToFee)
        {
            return new CalculationResult { Amount = 0m, Notes = "invoice payment; " };
        }
    }
}
