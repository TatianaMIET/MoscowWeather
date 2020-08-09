using System.Collections.Generic;
using System.Configuration;

namespace DataLayer
{
    public sealed class Settings
    {
        public List<string> HeaderNames { get; }

        public Settings()
        {
            HeaderNames = new List<string>(12)
            {
                ConfigurationManager.AppSettings["Date"],
                ConfigurationManager.AppSettings["Time"],
                ConfigurationManager.AppSettings["Temperature"],
                ConfigurationManager.AppSettings["Humidity"],
                ConfigurationManager.AppSettings["DewPoint"],
                ConfigurationManager.AppSettings["Pressure"],
                ConfigurationManager.AppSettings["WindDirection"],
                ConfigurationManager.AppSettings["WindSpeed"],
                ConfigurationManager.AppSettings["Cloudiness"],
                ConfigurationManager.AppSettings["CloudBase"],
                ConfigurationManager.AppSettings["HorizontalVisibility"],
                ConfigurationManager.AppSettings["WeatherPhenomena"]
            };
        }
    }
}