using System;
using System.Collections.Generic;
using LegacyRenewalApp.Calculators.Policies;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators
{
    public class DiscountCalculator : IDiscountCalculator
    {
        private readonly IEnumerable<IDiscountPolicy> _policies;

        public DiscountCalculator(IEnumerable<IDiscountPolicy> policies)
        {
            _policies = policies ?? throw new ArgumentNullException(nameof(policies));
        }

        public CalculationResult CalculateTotalDiscount(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, bool useLoyaltyPoints)
        {
            var totalDiscount = CalculationResult.Zero;

            foreach (var policy in _policies)
            {
                var partialDiscount = policy.CalculateDiscount(customer, plan, seatCount, baseAmount, useLoyaltyPoints);
                totalDiscount = totalDiscount.Add(partialDiscount);
            }

            return totalDiscount;
        }
    }
}
