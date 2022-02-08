using App.Models;

namespace App.Interfaces
{
    public interface ICompanyRepository
    {
        Company GetCompanyById(int companyId);
    }
}
