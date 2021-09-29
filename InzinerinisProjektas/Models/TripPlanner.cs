using static InzinerinisProjektas.Models.FlightsResponse;

namespace InzinerinisProjektas.Models
{
    public class TripPlanner
    {
        public string CurrentCountry { get; set; }
        public string DestinationCountry { get; set; }
        public CovidCasesResponse.Cases Cases { get; set; }
        public Place Places {  get; set; }
        public WeekWeatherForecast.Rootobject WeatherForecasts {  get; set; }
    }
}
