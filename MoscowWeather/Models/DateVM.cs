using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoscowWeather.Models
{
    /// <summary>
    /// Класс DateVM
    /// модель для отображения месяца и даты
    /// за которые сделана выборка по данным
    /// </summary>
    public class DateVM
    {
        public int? Year { get; set; }

        public int? Month { get; set; }
    }
}