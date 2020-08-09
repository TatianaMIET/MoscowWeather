using DataLayer.Codes;
using DataLayer.DBConnection;
using DataLayer.Entity;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataLayer.Services
{
    /// <summary>
    ///  Класс WeatherService
    ///  сервис для реализации логики
    ///  при обработке входящих запросов от
    ///  контроллера WeatherController
    /// </summary>
    public class WeatherService : IDisposable
    {
        private readonly WeatherDBContext db;
        private readonly ExelParseService exelParseService;

        public WeatherService()
        {
            db = new WeatherDBContext();
            exelParseService = new ExelParseService();
        }

        public List<string> ReadExelArchives(List<HttpPostedFileBase> archives)
        {
            List<string> result = new List<string>();
            foreach (var archive in archives)
            {
                if (archive.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    ExelParseResultModel archiveWeatherData = exelParseService.ReadExelArchive(archive);

                    if (archiveWeatherData.WeatherData.Count != 0)
                    {
                        db.WeatherData.AddRange(archiveWeatherData.WeatherData);
                        db.SaveChangesAsync();
                    }

                    if (archiveWeatherData.ErrorMessages.Count == 0)
                    {
                        result.Add(string.Format(ErrorTextTemplates.SUCCESS, archive.FileName));
                    }
                    else
                    {
                        foreach (var error in archiveWeatherData.ErrorMessages)
                        {
                            result.Add(error);
                        }
                    }
                }
                else
                {
                    result.Add(string.Format(ErrorTextTemplates.FILE_TYPE_ERROR, archive.FileName));
                }
            }
            return result;
        }

        public WeatherModel GetWeatherData(int? yearFilter, int? monthFilter, int page, int pageSize)
        {
            IQueryable<Weather> source = db.WeatherData.OrderBy(p => p.Date);

            if (yearFilter == null && monthFilter == null)
            {
                int count = source.Count();
                PageModel pageModel = new PageModel(count, page, pageSize);
                return new WeatherModel
                {
                    PageModel = pageModel,
                    WeatherData = source.Skip((page - 1) * pageSize).Take(pageSize).ToList()
                };

            }
            else 
            {
                int count = source.Count(w => w.Date.Year == yearFilter && (monthFilter == null? w.Date.Month >= 1 : w.Date.Month == monthFilter));
                PageModel pageModel = new PageModel(count, page, pageSize);
                return new WeatherModel
                {
                    PageModel = pageModel,
                    WeatherData = source.
                    Where(w => w.Date.Year == yearFilter && (monthFilter == null ? w.Date.Month >= 1 : w.Date.Month == monthFilter)).
                    Skip((page - 1) * pageSize).Take(pageSize).ToList()
                };
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
