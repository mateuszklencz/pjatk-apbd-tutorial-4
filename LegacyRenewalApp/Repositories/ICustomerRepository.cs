using System;

namespace LegacyRenewalApp.Repositories
{
    public interface ICustomerRepository
    {
        Customer GetById(int customerId);
    }
}
