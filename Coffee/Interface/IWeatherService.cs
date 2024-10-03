namespace Coffee.Interface
{
    public interface IWeatherService
    {
        Task<double> GetCurrentTemperatureAsync();
    }
}
