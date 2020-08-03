using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace DataLayer
{
    public sealed class Settings
    {
        public static string DATE { get; } = WebConfigurationManager.AppSettings["Date"];

        public static string TIME { get; } = WebConfigurationManager.AppSettings["Time"];

        public static string TEMPERATURE { get; } = WebConfigurationManager.AppSettings["Temperature"];

        public static string HUMIDITY { get; } = WebConfigurationManager.AppSettings["Humidity"];

        public static string DEW_POINT { get; } = WebConfigurationManager.AppSettings["DewPoint"];

        public static string PRESSURE { get; } = WebConfigurationManager.AppSettings["Pressure"];

        public static string WIND_DIRECTION { get; } = WebConfigurationManager.AppSettings["WindDirection"];

        public static string WIND_SPEED { get; } = WebConfigurationManager.AppSettings["WindSpeed"];

        public static string CLOUDINESS { get; } = WebConfigurationManager.AppSettings["Cloudiness"];

        public static string CLOUD_BASE { get; } = WebConfigurationManager.AppSettings["CloudBase"];

        public static string HORIZONTAL_VISIBILITY { get; } = WebConfigurationManager.AppSettings["HorizontalVisibility"];

        public static string WEATHER_PHENOMENA { get; } = WebConfigurationManager.AppSettings["WeatherPhenomena"];
    }
}