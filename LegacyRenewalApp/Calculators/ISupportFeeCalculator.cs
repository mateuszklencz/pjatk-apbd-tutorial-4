using System;

namespace LegacyRenewalApp.Calculators
{
    public interface ISupportFeeCalculator
    {
        Models.CalculationResult CalculateSupportFee(string planCode, bool includePremiumSupport);
    }
}
