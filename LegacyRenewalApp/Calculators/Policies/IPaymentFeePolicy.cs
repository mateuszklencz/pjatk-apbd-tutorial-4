using System;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators.Policies
{
    public interface IPaymentFeePolicy
    {
        bool IsApplicable(string paymentMethod);
        CalculationResult CalculateFee(decimal amountToFee);
    }
}
