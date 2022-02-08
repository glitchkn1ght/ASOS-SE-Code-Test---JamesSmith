namespace App.Interfaces
{
    using App.Models;

    public interface ICustomerRepositoryWrapper
    {
         void AddCustomerToDatabase(Customer customer);
    }
}
