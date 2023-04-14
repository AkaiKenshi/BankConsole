using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BankGetData.Customer;

public class Customer
{
    public async static Task<bool> FindIfUserNameAvailable(string userName)
    {
        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri($"http://localhost:5185/api/customers/AvailableUserName/{userName}"),
            Method = HttpMethod.Get
        };

        var response = await client.SendAsync(request);
        var result = bool.Parse(await response.Content.ReadAsStringAsync());

        return result;
    }

    public async static Task<bool> IsValidID(string id)
    {
        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri($"http://localhost:5185/api/customers/AvailableID/{id}"),
            Method = HttpMethod.Get
        };

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async static Task<bool> IsValidPassword(string userName, string password)
    {
        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri($"http://localhost:5185/api/customers/ValidateLogin/{userName}/{password}"),
            Method = HttpMethod.Get
        };

        var response = await client.SendAsync(request);
        var result = bool.Parse(await response.Content.ReadAsStringAsync());
        return result;
    }

    public async static Task<string> GetIdByUsername(string userName)
    {
        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri($"http://localhost:5185/api/customers/GetIdByUserName/{userName}"),
            Method = HttpMethod.Get
        };

        var response = await client.SendAsync(request);
        var result =await response.Content.ReadAsStringAsync();
        return result;
    }

    public static bool ValidatePassword(string password) => true;

    public async static Task CreateCustomer(string customer_id, string customer_user_name, string customer_name, string customer_last_name, string customer_password)
    {
        using var client = new HttpClient();

        var customer = new CreateCustomerRequest(customer_id, customer_user_name, customer_name, customer_last_name, customer_password);

        var request = new HttpRequestMessage
        {
            RequestUri = new Uri($"http://localhost:5185/api/customers/"),
            Method = HttpMethod.Post
        };

        var bodyString = JsonConvert.SerializeObject(customer);
        var content = new StringContent(bodyString, Encoding.UTF8, "application/json");
        request.Content = content;

        var response = await client.SendAsync(request);
        var result = await response.Content.ReadAsStringAsync();
    }

}
