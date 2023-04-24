using Newtonsoft.Json;
using System.Net.Http.Json;

namespace BankConsole.Data.Customer
{
    public class CustomerData
    {
        HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7035")
        };

        public async Task<bool> GetAvailableId(string id)
        {
            return await _client.GetFromJsonAsync<bool>($"/api/Customer/isCustomerIdAvailable/{id}");
        }
    }
}
