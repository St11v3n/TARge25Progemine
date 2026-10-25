using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TARge25Shop.Core.Dto.WeatherDtos;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.ApplicationServices.Services
{
    public class WeatherServices : IWeatherServices
    {
        private readonly HttpClient _httpClient;

        public WeatherServices
            (
                HttpClient httpClient
            )
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherRootDto> GetWeather(WeatherResultDto dto)
        {
            string apiKey = "d6971dff65d0e6aa59a394afac9de95b";
            var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={dto.City}&appid={apiKey}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var weather = JsonSerializer.Deserialize<WeatherRootDto>(json, options);

            return weather!;
        }
    }
}
