using DataLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    public class WeatherModel
    {
        public List<Weather> WeatherData { get; set; }

        public PageModel PageModel { get; set; }
    }
}
