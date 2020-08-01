using DataLayer.DBConnection;
using DataLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Services
{
    public class WeatherService : IDisposable
    {
        private readonly WeatherDBContext db;

        public WeatherService()
        {
            db = new WeatherDBContext();
        }


        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
