using System;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators.Policies
{
    public class SegmentDiscountPolicy : IDiscountPolicy
    {
        public CalculationResult CalculateDiscount(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, bool useLoyaltyPoints)
        {
            if (customer.Segment == "Silver")
                return new CalculationResult { Amount = baseAmount * 0.05m, Notes = "silver discount; " };
            if (customer.Segment == "Gold")
                return new CalculationResult { Amount = baseAmount * 0.10m, Notes = "gold discount; " };
            if (customer.Segment == "Platinum")
                return new CalculationResult { Amount = baseAmount * 0.15m, Notes = "platinum discount; " };
            if (customer.Segment == "Education" && plan.IsEducationEligible)
                return new CalculationResult { Amount = baseAmount * 0.20m, Notes = "education discount; " };

            return CalculationResult.Zero;
        }
    }
}
