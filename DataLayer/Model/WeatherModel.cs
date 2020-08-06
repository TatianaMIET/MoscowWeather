using DataLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    /// <summary>
    ///  Класс WeatherModel
    ///  модель для возврата в контроллер данных
    ///  для отображения
    /// </summary>

    public class WeatherModel
    {
        public List<Weather> WeatherData { get; set; }

        public PageModel PageModel { get; set; }
    }
}
