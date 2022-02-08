namespace App.Interfaces
{
    using App.Models;
    using System.Threading.Tasks;

    public interface ICustomerCreditOrchestrator
    {
        Task<Customer> SetCustomerCreditDetails(Customer customer);
    }
}
