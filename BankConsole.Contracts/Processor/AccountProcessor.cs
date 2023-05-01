using BankConsole.Contracts.DTOs.Accounts;
using BankConsole.Contracts.Models;
using Newtonsoft.Json;
using System.Text;

namespace BankConsole.Contracts.Processor;

public static class AccountProcessor
{
    public async static Task<Account> GetAccountFromIdAsync(string accountId, string token)
    {
        var url = $"/api/Account/{accountId}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Authorization", $"bearer {token}");

        using HttpResponseMessage respone = await ApiHelper.ApiClient.SendAsync(request);
        respone.EnsureSuccessStatusCode();  
        return await respone.Content.ReadAsAsync<Account>();
    }

    public async static Task<List<Account>> GetAccountsAsync(string token)
    {
        var url = "/api/Account/getAllAccounts";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Authorization", $"token {token}");

        using HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request); 
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<List<Account>>();
    }

    public async static Task<Account> CreateAccountAsync<T>(T createRequest, string url, string token)
    {
        var content = new StringContent(JsonConvert.SerializeObject(createRequest), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("Authorization", $"bearer {token}");
        request.Content = content;

        using HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request); 
        response.EnsureSuccessStatusCode();
        return await request.Content.ReadAsAsync<Account>();
    } 

    public async static Task TransactionBalanceAsync<T>(T updateRequest, string url, string token)
    {
        var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("Authorization", $"bearer {token}");
        request.Content = content;

        using HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
