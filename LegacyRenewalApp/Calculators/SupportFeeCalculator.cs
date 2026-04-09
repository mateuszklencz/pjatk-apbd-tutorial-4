using System;
using System.Collections.Generic;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators
{
    public class SupportFeeCalculator : ISupportFeeCalculator
    {
        private static readonly Dictionary<string, decimal> FeeMap = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
        {
            { "START", 250m },
            { "PRO", 400m },
            { "ENTERPRISE", 700m }
        };

        public CalculationResult CalculateSupportFee(string planCode, bool includePremiumSupport)
        {
            if (!includePremiumSupport)
            {
                return CalculationResult.Zero;
            }

            if (FeeMap.TryGetValue(planCode, out var fee))
            {
                return new CalculationResult { Amount = fee, Notes = "premium support included; " };
            }

            return CalculationResult.Zero;
        }
    }
}
