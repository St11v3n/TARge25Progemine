using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Core.Dto.CocktailDtos;


namespace TARge25Shop.ApplicationServices.Services
{
    public class CocktailServices : ICocktailServices
    {
        private readonly HttpClient _httpClient;

        public CocktailServices
            (
                HttpClient httpClient
            )
        {
            _httpClient = httpClient;
        }

        public async Task<CocktailRootDto> GetCocktails(CocktailResultDto dto)
        {
            string apiKey = "1";
            var response = await _httpClient.GetAsync($"https://www.thecocktaildb.com/api/json/v1/{apiKey}/search.php?s={dto.StrDrink}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var cocktail = JsonSerializer.Deserialize<CocktailRootDto>(json, options);

            return cocktail!;
        }
    }
}