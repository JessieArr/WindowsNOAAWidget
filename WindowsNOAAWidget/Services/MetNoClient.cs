using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WindowsNOAAWidget.Models.MetNo;

namespace WindowsNOAAWidget.Services
{
    public class MetNoClient
    {
        private HttpClient _httpClient;

        public MetNoClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<LocationForecast> GetWeatherInfoForPoint(double lat, double lon)
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "WindowsWeatherWidget");
            var result = await _httpClient.GetAsync($"https://api.met.no/weatherapi/locationforecast/2.0/complete?lat={lat}&lon={lon}");
            var response = await result.Content.ReadAsStringAsync();
            if (response.StartsWith("<"))
            {
                ErrorHelper.EmitError("Non-JSON response: " + response);
            }
            return JsonConvert.DeserializeObject<LocationForecast>(response);
        }
    }
}
