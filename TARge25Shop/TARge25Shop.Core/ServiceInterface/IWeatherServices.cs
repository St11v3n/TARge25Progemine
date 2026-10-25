using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TARge25Shop.Core.Dto.WeatherDtos;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface IWeatherServices
    {
        Task<WeatherRootDto> GetWeather(WeatherResultDto dto);
    }
}