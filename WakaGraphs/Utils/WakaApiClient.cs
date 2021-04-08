using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WakaGraphs.Models;

namespace WakaGraphs.Utils
{
    class WakaApiClient
    {
        readonly HttpClient client;
        readonly JsonSerializerOptions jsonSerializerOption;

        public WakaApiClient(string wakaTimeKey)
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri("https://wakatime.com/api/v1/")
            };

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(wakaTimeKey)));

            jsonSerializerOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        async Task<T> GetAsync<T>(string path) => JsonSerializer.Deserialize<T>(await client.GetStringAsync(path), jsonSerializerOption);

        public Task<AllTimeSinceToday> GetAllTimeDataAsync() => GetAsync<AllTimeSinceToday>("users/current/all_time_since_today");
    }
}
