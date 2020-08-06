using DataLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoscowWeather.Models
{
    /// <summary>
    /// Класс WeatherVM
    /// общая модель для отображения данных с учетом
    /// пагинации и фильтрации
    /// </summary>
    public class WeatherVM
    {
        public List<WeatherModel> WeatherData { get; set; }

        public PageVM PageViewModel { get; set; }

        public DateVM DateViewModel { get; set; }
    }
}