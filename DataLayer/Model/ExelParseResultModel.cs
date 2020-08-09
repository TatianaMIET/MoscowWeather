using DataLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    public class ExelParseResultModel
    {
        public List<Weather> WeatherData { get; set; }

        public List<string> ErrorMessages { get; set; }

        public ExelParseResultModel()
        {
            WeatherData = new List<Weather>();
            ErrorMessages = new List<string>();
        }
    }
}
