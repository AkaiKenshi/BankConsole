using BankConsole.Contracts.DTOs.Customers;
using BankConsole.Contracts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BankConsole.Contracts.Processor
{
    public class CustomerProcessor
    {
        public async Task<bool> IsCustomerIdAvailable(string id)
        {
            var url = $"/api/Customer/isCustomerIdAvailable/{id}";
            using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.ReasonPhrase);
            }
            return await response.Content.ReadAsAsync<bool>();
        }

        public async Task<bool> IsCustomerUsernameAvailable(string username)
        {
            var url = $"/api/Customer/isCustomerUsernameAvailable/{username}";
            using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url); 
            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.ReasonPhrase);
            }
            return await response.Content.ReadAsAsync<bool>();
        }

        public async Task<bool> IsCustomerEmailAvailable(string email)
        {
            var url = $"/api/Customer/isCustomerEmailAvailable/{email}"; 
            using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.ReasonPhrase);  
            }
            return await response.Content.ReadAsAsync<bool>();
        }

        public async Task<Customer> RegisterUser(CreateCustomerRequestDTO createRequest)
        {
            var url = "/api/Customer/Register";

            using HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(url, createRequest); 
            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.ReasonPhrase);
            }
            return await response.Content.ReadAsAsync<Customer>();
        }

        public async Task<Customer> LoginUser(GetCustomerLoginRequestDTO loginRequest)
        {
            var url = "/api/Customer/Login";

            using HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(url, loginRequest); 
            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.ReasonPhrase);
            }
            return await response.Content.ReadAsAsync<Customer>();
        }

        public async Task UpdateCustomerInformation(UpdateCustomerInformationRequestDTO updateRequest)
        {
            var url = "/api/Customer/UpdateCustomerInformation"; 

            using HttpResponseMessage response = await ApiHelper.ApiClient.PutAsJsonAsync(url, updateRequest);
            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.ReasonPhrase);
            }
        }
        
        public async Task UpdateCustomerUsername(UpdateCustomerUsernameRequestDTO updateRequest)
        {
            var url = "/api/Customer/UpdateCustomerInformation"; 

            using HttpResponseMessage response = await ApiHelper.ApiClient.PutAsJsonAsync(url, updateRequest);
            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.ReasonPhrase);
            }
        }

        public async Task UpdateCustomerEmail(UpdateCustomerEmailRequestDTO updateRequest)
        {
            var url = "/api/Customer/UpdateCustomerInformation"; 

            using HttpResponseMessage response = await ApiHelper.ApiClient.PutAsJsonAsync(url, updateRequest);
            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.ReasonPhrase);
            }
        }
        
        public async Task UpdateCustomerPassword(UpdateCustomerPasswordRequestDTO updateRequest)
        {
            var url = "/api/Customer/UpdateCustomerInformation"; 

            using HttpResponseMessage response = await ApiHelper.ApiClient.PutAsJsonAsync(url, updateRequest);
            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.ReasonPhrase);
            }
        }

        public async Task DeleteCustomer(string id)
        {
            var url = $"/api/Customer"; 

            using HttpResponseMessage response = await ApiHelper.ApiClient.DeleteAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.ReasonPhrase);
            }
        }

    }
}
