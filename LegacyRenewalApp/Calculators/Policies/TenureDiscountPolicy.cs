using System;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators.Policies
{
    public class TenureDiscountPolicy : IDiscountPolicy
    {
        public CalculationResult CalculateDiscount(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, bool useLoyaltyPoints)
        {
            if (customer.YearsWithCompany >= 5)
            {
                return new CalculationResult { Amount = baseAmount * 0.07m, Notes = "long-term loyalty discount; " };
            }
            if (customer.YearsWithCompany >= 2)
            {
                return new CalculationResult { Amount = baseAmount * 0.03m, Notes = "basic loyalty discount; " };
            }

            return CalculationResult.Zero;
        }
    }
}
