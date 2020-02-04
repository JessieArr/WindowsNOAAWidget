using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WindowsNOAAWidget.Models.Pollen;

namespace WindowsNOAAWidget.Services
{
    public class PollenClient
    {
        private HttpClient _httpClient;

        public PollenClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<PollenForecastResponse> GetPollenDataForZip(int zipCode)
        {
            _httpClient.DefaultRequestHeaders.Referrer = new Uri($"https://www.pollen.com/forecast/current/pollen/{zipCode}");
            var result = await _httpClient.GetAsync($"https://www.pollen.com/api/forecast/extended/pollen/{zipCode}");
            var response = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PollenForecastResponse>(response);
        }
    }
}
