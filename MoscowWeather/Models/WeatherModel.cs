using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoscowWeather.Models
{
    /// <summary>
    /// Класс WeatherModel
    /// модель для отображения данных сущности Weather
    /// </summary>
    public class WeatherModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Temperature { get; set; }

        public decimal Humidity { get; set; }

        public decimal DewPoint { get; set; }

        public short Pressure { get; set; }

        public string WindDirection { get; set; }

        public short? WindSpeed { get; set; }

        public short? Cloudiness { get; set; }

        public short CloudBase { get; set; }

        public short? HorizontalVisibility { get; set; }

        public string WeatherPhenomena { get; set; }
    }
}