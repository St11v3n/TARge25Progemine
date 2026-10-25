using System;
using System.Collections.Generic;

namespace TARge25Shop.Models.Weather
{
    public class WeatherViewModel
    {
        
        public string Name { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }

        
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

       
        public double Temp { get; set; }
        public double FeelsLike { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }

        
        public double Speed { get; set; }
        public int Deg { get; set; }
        public int All { get; set; }
    }
}