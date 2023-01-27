using APIProtect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Core.Interfaces;
using Weather.Core.Utilities;

namespace Weather.Core.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        public async Task<ResponseDto<IEnumerable<WeatherDTO>>> Get()
        {
            List<WeatherDTO> weatherList = new List<WeatherDTO>();
            var result = Enumerable.Range(4, 5).Select(index => new WeatherDTO
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToList();
            return ResponseDto<IEnumerable<WeatherDTO>>.Success("Login successful", result);

        }
    }
}
