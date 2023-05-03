using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankConsole.Contracts.Processor
{
    public static class TimeProcessor
    {
        public async static Task PassTime(int time)
        {
            var url = $"/api/Time/passTime/{time}";
            var content = new StringContent("");
            using HttpResponseMessage response = await ApiHelper.ApiClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
    }
}
