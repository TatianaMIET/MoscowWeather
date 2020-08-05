using System.Collections.Generic;
using System.Configuration;

namespace DataLayer
{
    public sealed class Settings
    {
        public static string DATE { get; } = ConfigurationManager.AppSettings["Date"];

        public static string TIME { get; } = ConfigurationManager.AppSettings["Time"];

        public static string TEMPERATURE { get; } = ConfigurationManager.AppSettings["Temperature"];

        public static string HUMIDITY { get; } = ConfigurationManager.AppSettings["Humidity"];

        public static string DEW_POINT { get; } = ConfigurationManager.AppSettings["DewPoint"];

        public static string PRESSURE { get; } = ConfigurationManager.AppSettings["Pressure"];

        public static string WIND_DIRECTION { get; } = ConfigurationManager.AppSettings["WindDirection"];

        public static string WIND_SPEED { get; } = ConfigurationManager.AppSettings["WindSpeed"];

        public static string CLOUDINESS { get; } = ConfigurationManager.AppSettings["Cloudiness"];

        public static string CLOUD_BASE { get; } = ConfigurationManager.AppSettings["CloudBase"];

        public static string HORIZONTAL_VISIBILITY { get; } = ConfigurationManager.AppSettings["HorizontalVisibility"];

        public static string WEATHER_PHENOMENA { get; } = ConfigurationManager.AppSettings["WeatherPhenomena"];
    }
}