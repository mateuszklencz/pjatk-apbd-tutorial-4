using System;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators
{
    public interface IPaymentFeeCalculator
    {
        CalculationResult CalculatePaymentFee(string paymentMethod, decimal amountToFee);
    }
}
