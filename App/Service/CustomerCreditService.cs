namespace App.Service
{
    using App.Interfaces;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class CustomerCreditService : ICustomerCreditService, IDisposable
    {
        private readonly HttpClient _apiClient;

        public CustomerCreditService()
        {
            _apiClient = new HttpClient();
        }

        public async Task<int> GetCreditLimit(string firstname, string surname, DateTime dateOfBirth)
        {
            return JObject.Parse(await (await _apiClient.PostAsync(ConfigurationManager.AppSettings["getCreditLimitEndpoint"], new StringContent("{\"fn\":\"" + firstname + "\",\"sn\":\"" + surname + "\",\"dob\":\"" + dateOfBirth + "\"}"))).Content.ReadAsStringAsync()).GetValue("creditLimit").Value<int>();
        }

        public void Dispose()
        {
            _apiClient.Dispose();
        }
    }
}