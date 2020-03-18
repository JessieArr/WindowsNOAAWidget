using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WindowsNOAAWidget.Models.NOAA;

namespace WindowsNOAAWidget.Services
{
    public class NOAAClient
    {
        private HttpClient _httpClient;

        public NOAAClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<PointResponse> GetWeatherInfoForPoint(double lat, double lon)
        {
            var header = new ProductHeaderValue("WindowsNOAAWidget");
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(header));
            var result = await _httpClient.GetAsync($"https://api.weather.gov/points/{lat},{lon}");
            var response = await result.Content.ReadAsStringAsync();
            if (response.StartsWith("<"))
            {
                ErrorHelper.EmitError("Non-JSON response: " + response);
            }
            return JsonConvert.DeserializeObject<PointResponse>(response);
        }

        public async Task<PointResponse> GetForecastForPoint(double lat, double lon)
        {
            var header = new ProductHeaderValue("WindowsNOAAWidget");
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(header));
            var result = await _httpClient.GetAsync($"https://api.weather.gov/points/{lat},{lon}/forecast");
            var response = await result.Content.ReadAsStringAsync();
            if (response.StartsWith("<"))
            {
                ErrorHelper.EmitError("Non-JSON response: " + response);
            }
            return JsonConvert.DeserializeObject<PointResponse>(response);
        }

        public async Task<PointResponse> GetHourlyForecastForPoint(double lat, double lon)
        {
            try
            {
                var header = new ProductHeaderValue("WindowsNOAAWidget");
                _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(header));
                var result = await _httpClient.GetAsync($"https://api.weather.gov/points/{lat},{lon}/forecast/hourly");
                var response = await result.Content.ReadAsStringAsync();
                if(response.StartsWith("<"))
                {
                    ErrorHelper.EmitError("Non-JSON response: " + response);
                }
                return JsonConvert.DeserializeObject<PointResponse>(response);
            }catch(Exception ex)
            {
                ErrorHelper.EmitError(ex);
                return null;
            }
        }

        public async Task<JObject> GetCountyZones()
        {
            var header = new ProductHeaderValue("WindowsNOAAWidget");
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(header));
            var result = await _httpClient.GetAsync($"https://api.weather.gov/zones?type=county");
            var response = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JObject>(response);
        }

        public async Task<Bitmap> GetImage(string url)
        {
            var header = new ProductHeaderValue("WindowsNOAAWidget");
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(header));
            var result = await _httpClient.GetAsync(url);
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                using (var stream = await result.Content.ReadAsStreamAsync())
                {
                    var memStream = new MemoryStream();
                    await stream.CopyToAsync(memStream);
                    memStream.Position = 0;
                    return new Bitmap(memStream);
                }
            }
            return null;
        }
    }
}
