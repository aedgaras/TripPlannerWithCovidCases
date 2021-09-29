namespace InzinerinisProjektas.Models
{
    public class CovidCasesResponse
    {
        public class Rootobject
        {
            public string Get { get; set; }
            public Parameters Parameters { get; set; }
            public object[] Errors { get; set; }
            public int Results { get; set; }
            public Response[] Response { get; set; }
        }

        public class Parameters
        {
            public string country { get; set; }
        }

        public class Response
        {
            public string continent { get; set; }
            public string country { get; set; }
            public int population { get; set; }
            public Cases cases { get; set; }
            public Deaths deaths { get; set; }
            public Tests tests { get; set; }
            public string day { get; set; }
            public DateTime time { get; set; }
        }

        public class Cases
        {
            public string New { get; set; }
            public int active { get; set; }
            public int critical { get; set; }
            public int recovered { get; set; }
            public string M_pop { get; set; }
            public int total { get; set; }
        }

        public class Deaths
        {
            public string _new { get; set; }
            public string _1M_pop { get; set; }
            public int total { get; set; }
        }

        public class Tests
        {
            public string _1M_pop { get; set; }
            public int total { get; set; }
        }

    }
}
