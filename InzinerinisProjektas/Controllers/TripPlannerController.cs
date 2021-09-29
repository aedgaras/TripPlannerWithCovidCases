using InzinerinisProjektas.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Globalization;

namespace InzinerinisProjektas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripPlannerController : ControllerBase
    {
        [HttpGet("destinationWeather/{city}")]
        public async Task<WeatherForecastResponse> GetDestinationWeather(string city)
        {
            var client = new RestClient($"https://community-open-weather-map.p.rapidapi.com/weather?q={city}&&units=metric");
            var request = new RestRequest(Method.GET);

            request.AddHeader("x-rapidapi-host", "community-open-weather-map.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "e4632e026fmsh22c0771086f2801p1e355djsn8d166deec3dc");

            IRestResponse response = client.Execute(request);

            var rootobject = JsonConvert.DeserializeObject<WeatherForecastResponse.Rootobject>(response.Content);
            var result = new WeatherForecastResponse();

            result.WeatherInfo = rootobject.weather.FirstOrDefault();
            result.Temperature = rootobject.main;

            return result;
        }

        [HttpGet("currentLocation")]
        public async Task<LocationResponse> GetCurrentLocation()
        {
            var client = new RestClient("https://find-any-ip-address-or-domain-location-world-wide.p.rapidapi.com/iplocation?apikey=873dbe322aea47f89dcf729dcc8f60e8");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "find-any-ip-address-or-domain-location-world-wide.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "e4632e026fmsh22c0771086f2801p1e355djsn8d166deec3dc");
            IRestResponse response = client.Execute(request);

            var result = JsonConvert.DeserializeObject<LocationResponse>(response.Content);
            return result;
        }

        [HttpGet("flightsToCity/{city}")]
        public async Task<FlightsResponse> GetFlightsToCity(string currentCountryCode, string city)
        {
            var client = new RestClient($"https://skyscanner-skyscanner-flight-search-v1.p.rapidapi.com/apiservices/autosuggest/v1.0/{currentCountryCode}/EUR/en-GB/?query={city}");
            var request = new RestRequest(Method.GET);

            request.AddHeader("x-rapidapi-host", "skyscanner-skyscanner-flight-search-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "e4632e026fmsh22c0771086f2801p1e355djsn8d166deec3dc");

            IRestResponse response = client.Execute(request);
            var result = JsonConvert.DeserializeObject<FlightsResponse>(response.Content);

            return result;
        }

        [HttpGet("covidCasesInCountry/{country}")]
        public async Task<CovidCasesResponse.Cases> GetCovidCasesByCountry(string country)
        {
            var client = new RestClient($"https://covid-193.p.rapidapi.com/statistics?country={country}");
            var request = new RestRequest(Method.GET);

            request.AddHeader("x-rapidapi-host", "covid-193.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "e4632e026fmsh22c0771086f2801p1e355djsn8d166deec3dc");
            IRestResponse response = client.Execute(request);

            var covidCasesResponse = JsonConvert.DeserializeObject<CovidCasesResponse.Rootobject>(response.Content);
            var casesResult = covidCasesResponse.Response.First().cases;

            return casesResult;
        }

        [HttpGet("tripToCityInfo/{city}")]
        public async Task<TripPlanner> GetTripToCityInfo(string city)
        {
            var currentLocation = GetCurrentLocation().Result;
            var flightsToCity = GetFlightsToCity(currentLocation.countryISO2, city).Result.Places;

            var weather = GetWeekWeatherForecast(flightsToCity.First().CountryName).Result;

            var destinationCountryCode = flightsToCity.First().CountryId.Substring(0, flightsToCity.First().CountryId.IndexOf('-'));
            var cases = new CovidCasesResponse.Cases();

            try
            {
                cases = GetCovidCasesByCountry(flightsToCity.First().CountryName).Result;

            }
            catch (Exception)
            {

                cases = GetCovidCasesByCountry(destinationCountryCode).Result;
            }


            return new TripPlanner 
            { 
                CurrentCountry = currentLocation.country, 
                DestinationCountry = flightsToCity.First().CountryName, 
                Places = flightsToCity.FirstOrDefault(), 
                WeatherForecasts = weather, 
                Cases = cases
            };
        }

        [HttpGet("weekWeatherForecast/{city}")]
        public async Task<WeekWeatherForecast.Rootobject> GetWeekWeatherForecast(string city)
        {
            var client = new RestClient("https://community-open-weather-map.p.rapidapi.com/forecast/daily?q=london&units=metric");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "community-open-weather-map.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "e4632e026fmsh22c0771086f2801p1e355djsn8d166deec3dc");
            IRestResponse response = client.Execute(request);

            var weeksForecast = JsonConvert.DeserializeObject<WeekWeatherForecast.Rootobject>(response.Content);

            return weeksForecast;
        }

    }
}
