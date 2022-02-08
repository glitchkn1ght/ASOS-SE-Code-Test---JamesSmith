namespace App.Interfaces
{
    using System;
    using System.Threading.Tasks;

    public interface ICustomerCreditService
    {
        Task<int> GetCreditLimit(string firstname, string surname, DateTime dateOfBirth);
    }

}
