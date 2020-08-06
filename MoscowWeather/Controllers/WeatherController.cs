using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using AutoMapper;
using DataLayer.DBConnection;
using DataLayer.Entity;
using DataLayer.Model;
using DataLayer.Services;
using MoscowWeather.Models;
using NLog;

namespace MoscowWeather.Controllers
{
    /// <summary>
    /// Класс WeatherController
    /// контроллер, принимающий и обрабатывающий 
    /// входящие запросы пользователей
    /// </summary>
    public class WeatherController : Controller
    {
        private readonly WeatherService weatherService = new WeatherService();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return View();
        }

        // GET: Weathers
        public ActionResult GetWeatherData(int? year, int? month, int page=1)
        {
            int pageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"]);

            #region Mapper
            var config = new MapperConfiguration(map =>
            {
                map.CreateMap<Weather, MoscowWeather.Models.WeatherModel>();
                map.CreateMap<PageModel, PageVM>();
                map.CreateMap<DataLayer.Model.WeatherModel, WeatherVM>()
                .ForMember("PageViewModel", opt => opt.MapFrom(c =>  c.PageModel));
            });
            var mapper = new Mapper(config);
            #endregion

            DataLayer.Model.WeatherModel  weatherViewModel 
                = weatherService.GetWeatherData(year, year==null ? null: month, page, pageSize);

            DateVM dateViewModel = new DateVM
            {
                Year = year,
                Month = month
            };

            WeatherVM weatherVM = mapper.Map<WeatherVM>(weatherViewModel);
            weatherVM.DateViewModel = dateViewModel;

            return View("Weather", weatherVM);
            
        }


        public ActionResult DownloadWeatherArchives()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DownloadWeatherArchives(List<HttpPostedFileBase> archives)
        {

            ErrorMessagesModel model = new ErrorMessagesModel();

            List<string> result = weatherService.ReadExelArchives(archives);
            model.ErrorMessages = result;

            return View("DownloadResults", model);

        }

    }
}
