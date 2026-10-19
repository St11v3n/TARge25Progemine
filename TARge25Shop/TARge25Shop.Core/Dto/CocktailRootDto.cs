using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TARge25Shop.Core.Dto
{
    public class CocktailRootDto
    {
        [JsonPropertyName("Drinks")]
        public List<string> Drinks { get; set; }

        [JsonPropertyName("idDrink")]
        public string Id { get; set; }

        [JsonPropertyName("strDrink")]
        public string Name { get; set; }

        [JsonPropertyName("strDrinkAlternate")]
        public string Alternative { get; set; }

        [JsonPropertyName("strTags")]
        public string Tags { get; set; }

        [JsonPropertyName("strVideo")]
        public string Video { get; set; }

        [JsonPropertyName("strCategory")]
        public string Category { get; set; }

        [JsonPropertyName("strIBA")]
        public string IBA { get; set; }

        [JsonPropertyName("strAlcoholic")]
        public string Alcoholic { get; set; }

        [JsonPropertyName("strGlass")]
        public string Glass { get; set; }

        [JsonPropertyName("strInstructions")]
        public string Instructions { get; set; }
    }
}
