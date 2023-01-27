using Weather.Core.Utilities;

namespace Weather.Core.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<ResponseDto<IEnumerable<WeatherDTO>>> Get();
    }
}