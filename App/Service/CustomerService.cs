namespace App.Service
{
    using App.BusinessLogic;
    using App.DAL;
    using App.Interfaces;
    using App.Models;
    using App.Validation;
    using System;

    public class CustomerService
    {
        private readonly ICustomerValidator CustomerValidator;

        private readonly ICompanyRepository CompanyRepository;

        private readonly ICustomerCreditOrchestrator CustomerCreditOrchestrator;

        private readonly ICustomerRepositoryWrapper CustomerDataAccessWrapper;

        public CustomerService()
        {
            this.CustomerValidator = new CustomerValidator();
            this.CompanyRepository = new CompanyRepository();
            this.CustomerCreditOrchestrator = new CustomerCreditOrchestrator();
            this.CustomerDataAccessWrapper = new CustomerRepositoryWrapper();
        }

        public CustomerService(ICustomerValidator customerValidator, ICompanyRepository companyRepository, ICustomerCreditOrchestrator customerCreditOrchestrator, ICustomerRepositoryWrapper customerDataAccessWrapper)
        {
            this.CustomerValidator = customerValidator;
            this.CompanyRepository = companyRepository;
            this.CustomerCreditOrchestrator = customerCreditOrchestrator;
            this.CustomerDataAccessWrapper = customerDataAccessWrapper;
        }

        public bool AddCustomer(string firstName, string lastName, string emailAddress, DateTime dateOfBirth, int companyId)
        {
            var proposedCustomer = new Customer
            {
                Firstname = firstName,
                Lastname = lastName,
                DateOfBirth = dateOfBirth,
                EmailAddress = emailAddress
            };

            if (!this.CustomerValidator.ValidateCustomerDetails(proposedCustomer))
            {
                return false;
            }

            var company = this.CompanyRepository.GetCompanyById(companyId);

            if (company == null)
            {
                return false;
            }

            proposedCustomer.Company = company;

            proposedCustomer = this.CustomerCreditOrchestrator.SetCustomerCreditDetails(proposedCustomer).Result;

            if(!this.CustomerValidator.ValidateCustomerCreditDetails(proposedCustomer))
            {
                return false;
            }

            this.CustomerDataAccessWrapper.AddCustomerToDatabase(proposedCustomer);

            return true;
        }
    }
}
