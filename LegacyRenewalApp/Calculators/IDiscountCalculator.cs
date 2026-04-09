using System;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators
{
    public interface IDiscountCalculator
    {
        CalculationResult CalculateTotalDiscount(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, bool useLoyaltyPoints);
    }
}
