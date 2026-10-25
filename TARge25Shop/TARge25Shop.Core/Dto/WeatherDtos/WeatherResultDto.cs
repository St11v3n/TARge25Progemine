using System;
using System.Collections.Generic;
using System.Text;

namespace TARge25Shop.Core.Dto.WeatherDtos
{
    public class WeatherResultDto
    {
        
        public string City { get; set; }

        
        public double Temp { get; set; }
        public double FeelsLike { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
        public double WindSpeed { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}