using System;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators.Policies
{
    public class LoyaltyPointsDiscountPolicy : IDiscountPolicy
    {
        public CalculationResult CalculateDiscount(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, bool useLoyaltyPoints)
        {
            if (useLoyaltyPoints && customer.LoyaltyPoints > 0)
            {
                int pointsToUse = customer.LoyaltyPoints > 200 ? 200 : customer.LoyaltyPoints;
                return new CalculationResult 
                { 
                    Amount = pointsToUse, 
                    Notes = $"loyalty points used: {pointsToUse}; " 
                };
            }

            return CalculationResult.Zero;
        }
    }
}
