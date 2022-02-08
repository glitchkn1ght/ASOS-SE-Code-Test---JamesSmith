namespace App.BusinessLogic
{
    using App.Interfaces;
    using App.Models;
    using App.Service;
    using System.Threading.Tasks;

    public class CustomerCreditOrchestrator : ICustomerCreditOrchestrator
    {
        private readonly ICustomerCreditService CustomerCreditService;

        public CustomerCreditOrchestrator()
        {
            this.CustomerCreditService = new CustomerCreditService();
        }
        
        public CustomerCreditOrchestrator(ICustomerCreditService customerCreditService)
        {
            this.CustomerCreditService = customerCreditService ; 
        }

        public async Task<Customer> SetCustomerCreditDetails(Customer customer)
        {
            if (customer.Company.Name == "VeryImportantClient")
            {
                // Skip credit check
                customer.HasCreditLimit = false;
                return customer;
            }

            customer.HasCreditLimit = true;
            int creditLimit = customer.CreditLimit = await this.CustomerCreditService.GetCreditLimit(customer.Firstname, customer.Lastname, customer.DateOfBirth);

            if (customer.Company.Name == "ImportantClient")
            {
                //Double Limit for these guys
               creditLimit = creditLimit * 2;
            }

            customer.CreditLimit = creditLimit;

            return customer;
        }
    }
}
