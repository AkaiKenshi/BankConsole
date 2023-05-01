using BankConsole.Contracts.DTOs.Customers;
using BankConsole.Contracts.Models;
using Newtonsoft.Json;
using System.Text;

namespace BankConsole.Contracts.Processor
{
    public static class CustomerProcessor
    {
        public static async Task<bool> IsCustomerIdAvailableAsync(string id)
        {
            var url = $"/api/Customer/isCustomerIdAvailable/{id}";
            using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<bool>();
        }

        public static async Task<bool> IsCustomerUsernameAvailableAsync(string username)
        {
            var url = $"/api/Customer/isCustomerUsernameAvailable/{username}";
            using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<bool>();
        }

        public static async Task<bool> IsCustomerEmailAvailableAsync(string email)
        {
            var url = $"/api/Customer/isCustomerEmailAvailable/{email}";
            using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<bool>();
        }

        public static async Task<Customer> RegisterUserAsync(CreateCustomerRequestDTO createRequest)
        {
            var url = "/api/Customer/Register";

            using HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(url, createRequest);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Customer>();
        }

        public static async Task<Customer> LoginUserAsync(GetCustomerLoginRequestDTO loginRequest)
        {
            var url = "/api/Customer/Login";

            using HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(url, loginRequest);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Customer>();
        }

        public static async Task UpdateCustomerInformationAsync(UpdateCustomerInformationRequestDTO updateRequest, string token)
        {
            var uri = new Uri("/api/Customer/UpdateCustomerInformation");
            var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Headers.Add("Authorization", $"bearer {token}");
            request.Content = content;

            using HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        public static async Task UpdateCustomerUsernameAsync(UpdateCustomerUsernameRequestDTO updateRequest, string token)
        {
            var uri = new Uri("/api/Customer/UpdateCustomerInformation");
            var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Headers.Add("Authorization", $"bearer {token}");
            request.Content = content;

            using HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public static async Task UpdateCustomerEmailAsync(UpdateCustomerEmailRequestDTO updateRequest, string token)
        {
            var uri = new Uri("/api/Customer/UpdateCustomerInformation");
            var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Headers.Add("Authorization", $"bearer {token}");
            request.Content = content;

            using HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public static async Task UpdateCustomerPasswordAsync(UpdateCustomerPasswordRequestDTO updateRequest, string token)
        {
            var uri = new Uri("/api/Customer/UpdateCustomerInformation");
            var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Headers.Add("Authorization", $"bearer {token}");
            request.Content = content;

            using HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

         static async Task DeleteCustomerAsync(string token)
        {
            var uri = new Uri($"/api/Customer");
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
            request.Headers.Add("Authorization", $"bearer {token}");

            using HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

    }
}
