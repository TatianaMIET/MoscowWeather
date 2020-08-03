using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using DataLayer.DBConnection;
using DataLayer.Entity;
using DataLayer.Services;
using NLog;

namespace MoscowWeather.Controllers
{
    public class WeatherController : Controller
    {
        private readonly WeatherDBContext db = new WeatherDBContext();
        private readonly WeatherService weatherService = new WeatherService();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Weathers
        public ActionResult Index()
        {
            return View("Weather", db.WeatherData.ToList());
        }


        public ActionResult DownloadWeatherArchives()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DownloadWeatherArchives(List<HttpPostedFileBase> archives)
        {
           
            weatherService.ReadExelArchives(archives);
            return RedirectToAction("Index");

        } 


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Данные
        private readonly IMappingEngine mapping;

        private ISettings settings;
        #endregion
    }
}
