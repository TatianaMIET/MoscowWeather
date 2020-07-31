using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DataLayer.DBConnection;
using DataLayer.Entity;
using DataLayer.Services;

namespace MoscowWeather.Controllers
{
    public class WeatherController : Controller
    {
        private readonly WeatherDBContext db = new WeatherDBContext();
        private readonly WeatherService weatherService = new WeatherService();

        // GET: Weathers
        public ActionResult Index()
        {
            db.WeatherData.Add(
                new Weather
                {
                    CloudBase = 1,
                    Cloudiness = 1,
                    Date = DateTime.Now,
                    DewPoint = 1.0M,
                    HorizontalVisibility = 100,
                    Humidity = 100,
                    WindSpeed = 10,
                    Pressure = 1,
                    WindDirection = "a",
                    Temperature = -1.1M,
                    WeatherPhenomena = "a"
                });
            db.SaveChanges();
            return View("Weather", db.WeatherData.ToList());
        }


        public ActionResult DownloadWeatherArchives()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DownloadWeatherArchives([Bind(Include = "archives")] object archives)
        {
            int i = 0;
            return View("Weather");

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
