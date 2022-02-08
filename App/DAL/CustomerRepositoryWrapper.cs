namespace App.DAL
{
    using App.Interfaces;
    using App.Models;
    
    public class CustomerRepositoryWrapper : ICustomerRepositoryWrapper
    {
        public void AddCustomerToDatabase(Customer customer)
        {
            CustomerRepository.AddCustomer(customer);
        }
    }
}
