using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;

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
        public async Task<CocktailRootDto> CocktailResultHttpClient()
        {
            //var response on vaja ühendada https://www.thecocktaildb.com/api/json/v1/1/search.php?s=margarita
            var response = await _httpClient.GetAsync("https://www.thecocktaildb.com/api/json/v1/1/search.php?s=margarita");
            //annab veateate, kui response ei ole edukas
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                //olek true laseb ignoreerida täpset tähe suurustundlikust JSON muutujate nimes
                PropertyNameCaseInsensitive = true
            };
            //muutuja var joke tuleb deserialiseerida 

            var drink = JsonSerializer.Deserialize<CocktailRootDto>(json, options);
            //null andestav operaator(!) on turvaline viis kuna API alati tagastab JSON 
            return drink!;
        }
    }
}
