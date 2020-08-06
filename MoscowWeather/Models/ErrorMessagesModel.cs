using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoscowWeather.Models
{
    /// <summary>
    /// Класс ErrorMessagesModel
    /// модель для отображения списка ошибок,
    /// возникших при парсинге файлов
    /// </summary>
    public class ErrorMessagesModel
    {
        public List<string> ErrorMessages { get; set; }
    }
}