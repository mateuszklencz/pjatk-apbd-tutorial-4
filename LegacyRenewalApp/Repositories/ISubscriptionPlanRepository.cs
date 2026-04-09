using System;

namespace LegacyRenewalApp.Repositories
{
    public interface ISubscriptionPlanRepository
    {
        SubscriptionPlan GetByCode(string code);
    }
}
