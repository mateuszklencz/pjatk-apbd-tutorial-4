using System;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators.Policies
{
    public class VolumeDiscountPolicy : IDiscountPolicy
    {
        public CalculationResult CalculateDiscount(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, bool useLoyaltyPoints)
        {
            if (seatCount >= 50)
            {
                return new CalculationResult { Amount = baseAmount * 0.12m, Notes = "large team discount; " };
            }
            if (seatCount >= 20)
            {
                return new CalculationResult { Amount = baseAmount * 0.08m, Notes = "medium team discount; " };
            }
            if (seatCount >= 10)
            {
                return new CalculationResult { Amount = baseAmount * 0.04m, Notes = "small team discount; " };
            }

            return CalculationResult.Zero;
        }
    }
}
