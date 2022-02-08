namespace App.Validation
{
    using App.Interfaces;
    using App.Models;
    using System;

    public class CustomerValidator : ICustomerValidator
    {
        public bool ValidateCustomerDetails(Customer propoosedCustomer)
        {
            if (string.IsNullOrWhiteSpace(propoosedCustomer.Firstname) || string.IsNullOrWhiteSpace(propoosedCustomer.Lastname))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(propoosedCustomer.EmailAddress) || !(propoosedCustomer.EmailAddress.Contains("@")) || !(propoosedCustomer.EmailAddress.Contains(".")))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - propoosedCustomer.DateOfBirth.Year;
            
            if (now.Month < propoosedCustomer.DateOfBirth.Month || (now.Month == propoosedCustomer.DateOfBirth.Month && now.Day < propoosedCustomer.DateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }

            return true;
        }

        public bool ValidateCustomerCreditDetails(Customer proposedCustomer)
        {
            //In reality this limit would be configurable via db or maybe config file, but i thought it was kind of out of scope for this exercise
            if (proposedCustomer.HasCreditLimit && proposedCustomer.CreditLimit < 500)
            {
                return false;
            }

            return true;
        }
    }
}
