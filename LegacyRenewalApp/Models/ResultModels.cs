using System;

namespace LegacyRenewalApp.Models
{
    public class CalculationResult
    {
        public decimal Amount { get; set; }
        public string Notes { get; set; } = string.Empty;

        public static CalculationResult Zero => new CalculationResult { Amount = 0m, Notes = string.Empty };

        public CalculationResult Add(CalculationResult other)
        {
            return new CalculationResult
            {
                Amount = this.Amount + other.Amount,
                Notes = string.IsNullOrEmpty(this.Notes) ? other.Notes : 
                        string.IsNullOrEmpty(other.Notes) ? this.Notes : 
                        this.Notes + other.Notes
            };
        }
    }
}
