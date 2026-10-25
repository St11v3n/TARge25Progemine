using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TARge25Shop.Core.Dto.WeatherDtos;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.Weather; 

namespace TARge25Shop.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherServices _weatherServices;

        public WeatherController(IWeatherServices weatherServices)
        {
            _weatherServices = weatherServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchWeather(SearchWeatherViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                return RedirectToAction("Weather", "Weather", new { city = model.SearchCity });
            }

            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> Weather(string city)
        {
            WeatherResultDto dto = new()
            {
                City = city
            };

         
            var weatherResult = await _weatherServices.GetWeather(dto);

            if (weatherResult == null)
            {
                return RedirectToAction(nameof(Index));
            }

            
            WeatherViewModel vm = new()
            {
                Name = weatherResult.Name,
                Lon = weatherResult.Coord.Lon,
                Lat = weatherResult.Coord.Lat,
                Main = weatherResult.Weather[0].Main,
                Description = weatherResult.Weather[0].Description,
                Icon = weatherResult.Weather[0].Icon,
                Temp = weatherResult.Main.Temp,
                FeelsLike = weatherResult.Main.FeelsLike,
                TempMin = weatherResult.Main.TempMin,
                TempMax = weatherResult.Main.TempMax,
                Pressure = weatherResult.Main.Pressure,
                Humidity = weatherResult.Main.Humidity,
                Speed = weatherResult.Wind.Speed,
                Deg = weatherResult.Wind.Deg,
                All = weatherResult.Clouds.All
            };

            return View(vm);
        }
    }
}