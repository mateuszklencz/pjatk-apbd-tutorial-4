using System;
using System.Collections.Generic;

namespace LegacyRenewalApp.Calculators
{
    public class TaxCalculator : ITaxCalculator
    {
        private static readonly Dictionary<string, decimal> RatesMap = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
        {
            { "Poland", 0.23m },
            { "Germany", 0.19m },
            { "Czech Republic", 0.21m },
            { "Norway", 0.25m }
        };

        public decimal CalculateTaxRate(string country)
        {
            if (RatesMap.TryGetValue(country, out var rate))
            {
                return rate;
            }

            return 0.20m; // Default tax rate if country is not in map
        }
    }
}
