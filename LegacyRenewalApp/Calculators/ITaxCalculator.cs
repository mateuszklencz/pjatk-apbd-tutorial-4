using System;

namespace LegacyRenewalApp.Calculators
{
    public interface ITaxCalculator
    {
        decimal CalculateTaxRate(string country);
    }
}
