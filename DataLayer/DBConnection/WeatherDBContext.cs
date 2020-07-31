using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entity;

namespace DataLayer.DBConnection
{
    public class WeatherDBContext : DbContext
    {
        public DbSet<Weather> WeatherData { get; set; }

        public WeatherDBContext()   
        : base("ConnectionString") { }
    } 

}
