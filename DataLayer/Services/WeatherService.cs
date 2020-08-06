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
                    Dictionary<string, object> archiveWeatherData = exelParseService.ReadExelArchive(archive);

                    if (archiveWeatherData.ContainsKey("result"))
                    {
                        if (archiveWeatherData["result"] != null)
                        {
                            db.WeatherData.AddRange((List<Weather>)archiveWeatherData["result"]);
                            db.SaveChangesAsync();
                        }
                    }

                    if (archiveWeatherData.ContainsKey("header_error"))
                    {
                        result.Add(string.Format(ErrorTextTemplates.INCORRECT_HEADER, archive.FileName));
                    }
                    if (archiveWeatherData.ContainsKey("parse_error"))
                    {
                        result.Add(string.Format(ErrorTextTemplates.PARSE_ERROR, archive.FileName));
                    }
                    if (!(archiveWeatherData.ContainsKey("parse_error") || archiveWeatherData.ContainsKey("header_error")))
                    {
                        result.Add(string.Format(ErrorTextTemplates.SUCCESS, archive.FileName));
                    }
                }
                else
                {
                    result.Add(string.Format(ErrorTextTemplates.FILE_TYPE_ERROR, archive.FileName));
                }
            }
            return result;
        }

        public  WeatherModel GetWeatherData(int? yearFilter, int? monthFilter, int page, int pageSize)
        {
            IQueryable<Weather> source = db.WeatherData.OrderBy(p => p.Date);
            var count = source.CountAsync();
            PageModel pageModel = new PageModel(count.Result, page, pageSize);
            if (yearFilter == null && monthFilter == null)
            {
                return new WeatherModel
                {
                    PageModel = pageModel,
                    WeatherData = source.Skip((page - 1) * pageSize).Take(pageSize).ToList()
                };

            }
            else
            {
                return new WeatherModel
                {
                    PageModel = pageModel,
                    WeatherData = source.
                    Where(w => w.Date.Year == yearFilter && w.Date.Month == (monthFilter ?? 1)).
                    Skip((page - 1) * pageSize).Take(pageSize).ToList()
                };
            }
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
