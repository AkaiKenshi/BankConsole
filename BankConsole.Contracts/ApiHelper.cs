using System.Net.Http.Headers;

namespace BankConsole.Contracts;

public static class ApiHelper
{
    public static HttpClient ApiClient { get; set; } = null!; 

    public static void InnitalizeClient()
    {
        ApiClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7035/")
        };
        ApiClient.DefaultRequestHeaders.Accept.Clear();
        ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}
