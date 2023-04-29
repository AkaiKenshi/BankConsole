using BankConsole.Contracts.DTOs.Customers;
using BankConsole.Contracts.Models;
using Newtonsoft.Json;
using System.Text;

namespace BankConsole.Contracts.Processor
{
    public class CustomerProcessor
    {
        public async Task<bool> IsCustomerIdAvailable(string id)
        {
            var url = $"/api/Customer/isCustomerIdAvailable/{id}";
            using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<bool>();
        }

        public async Task<bool> IsCustomerUsernameAvailable(string username)
        {
            var url = $"/api/Customer/isCustomerUsernameAvailable/{username}";
            using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<bool>();
        }

        public async Task<bool> IsCustomerEmailAvailable(string email)
        {
            var url = $"/api/Customer/isCustomerEmailAvailable/{email}";
            using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<bool>();
        }

        public async Task<Customer> RegisterUser(CreateCustomerRequestDTO createRequest)
        {
            var url = "/api/Customer/Register";

            using HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(url, createRequest);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Customer>();
        }

        public async Task<Customer> LoginUser(GetCustomerLoginRequestDTO loginRequest)
        {
            var url = "/api/Customer/Login";

            using HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(url, loginRequest);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Customer>();
        }

        public async Task UpdateCustomerInformation(UpdateCustomerInformationRequestDTO updateRequest, string token)
        {
            var uri = new Uri("/api/Customer/UpdateCustomerInformation");
            var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Headers.Add("Authorization", $"bearer {token}");
            request.Content = content;

            using HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCustomerUsername(UpdateCustomerUsernameRequestDTO updateRequest, string token)
        {
            var uri = new Uri("/api/Customer/UpdateCustomerInformation");
            var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Headers.Add("Authorization", $"bearer {token}");
            request.Content = content;

            using HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCustomerEmail(UpdateCustomerEmailRequestDTO updateRequest, string token)
        {
            var uri = new Uri("/api/Customer/UpdateCustomerInformation");
            var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Headers.Add("Authorization", $"bearer {token}");
            request.Content = content;

            using HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCustomerPassword(UpdateCustomerPasswordRequestDTO updateRequest, string token)
        {
            var uri = new Uri("/api/Customer/UpdateCustomerInformation");
            var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Headers.Add("Authorization", $"bearer {token}");
            request.Content = content;

            using HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCustomer(string token)
        {
            var uri = new Uri($"/api/Customer");
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
            request.Headers.Add("Authorization", $"bearer {token}");

            using HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

    }
}
