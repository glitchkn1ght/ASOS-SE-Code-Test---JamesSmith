namespace App.Validation
{
    using App.Interfaces;
    using App.Models;
    using System;

    public class CustomerValidator : ICustomerValidator
    {
        public bool ValidateCustomerDetails(Customer proposedCustomer)
        {
            if (string.IsNullOrWhiteSpace(proposedCustomer.Firstname) || string.IsNullOrWhiteSpace(proposedCustomer.Lastname))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(proposedCustomer.EmailAddress) || !(proposedCustomer.EmailAddress.Contains("@")) || !(proposedCustomer.EmailAddress.Contains(".")))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - proposedCustomer.DateOfBirth.Year;
            
            if (now.Month < proposedCustomer.DateOfBirth.Month || (now.Month == proposedCustomer.DateOfBirth.Month && now.Day < proposedCustomer.DateOfBirth.Day)) age--;

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
