namespace App.Interfaces
{
    using App.Models;
    using System;

    public interface ICustomerValidator
    {
        bool ValidateCustomerDetails(Customer proposedCustomer);

        bool ValidateCustomerCreditDetails(Customer proposedCustomer);
    }
}
