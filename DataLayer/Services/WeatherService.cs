using DataLayer.Codes;
using DataLayer.DBConnection;
using DataLayer.Entity;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Profile;

namespace DataLayer.Services
{
    public class WeatherService : IDisposable
    {
        private readonly WeatherDBContext db;

        public WeatherService()
        {
            db = new WeatherDBContext();
        }

        public int ReadExelArchives(List<HttpPostedFileBase> archives)
        {
            XSSFWorkbook xssfwb = new XSSFWorkbook(archives[0].InputStream);

            ISheet sheet = xssfwb.GetSheetAt(0);

            //TODO: вынести значения строк с заголовком в конфиг
            IRow firstHeadingRow = sheet.GetRow(3);
            IRow secondHeadingRow = sheet.GetRow(4);

            //TODO: проверить условие
            if (firstHeadingRow.LastCellNum == secondHeadingRow.LastCellNum)
            {
                List<string> heading = new List<string>(firstHeadingRow.LastCellNum);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i <= firstHeadingRow.LastCellNum; i++)
                {
                    sb.Clear();
                    sb.Append(firstHeadingRow.GetCell(i));
                    sb.Append(" ");
                    sb.Append(secondHeadingRow.GetCell(i));
                    heading.Add(sb.ToString());
                }

                //Проверяем на соответствие столбцов порядку и содержнию заголовков архивов
                bool isCorrect = true;
                for (int i = 0; i <= heading.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            if (!heading[i].Equals(Settings.DATE))
                                isCorrect = false;
                            break;
                        case 1:
                            if (!heading[i].Equals(Settings.TIME))
                                isCorrect = false;
                            break;
                        case 2:
                            if (!heading[i].Equals(Settings.TEMPERATURE))
                                isCorrect = false;
                            break;
                        case 3:
                            if (!heading[i].Equals(Settings.HUMIDITY))
                                isCorrect = false;
                            break;
                        case 4:
                            if (!heading[i].Equals(Settings.DEW_POINT))
                                isCorrect = false;
                            break;
                        case 5:
                            if (!heading[i].Equals(Settings.PRESSURE))
                                isCorrect = false;
                            break;
                        case 6:
                            if (!heading[i].Equals(Settings.WIND_DIRECTION))
                                isCorrect = false;
                            break;
                        case 7:
                            if (!heading[i].Equals(Settings.WIND_SPEED))
                                isCorrect = false;
                            break;
                        case 8:
                            if (!heading[i].Equals(Settings.CLOUDINESS))
                                isCorrect = false;
                            break;
                        case 9:
                            if (!heading[i].Equals(Settings.CLOUD_BASE))
                                isCorrect = false;
                            break;
                        case 10:
                            if (!heading[i].Equals(Settings.HORIZONTAL_VISIBILITY))
                                isCorrect = false;
                            break;
                        case 11:
                            if (!heading[i].Equals(Settings.WEATHER_PHENOMENA))
                                isCorrect = false;
                            break;
                        default:
                            break;
                    }

                    if (!isCorrect)
                    {
                        return ErrorCodes.INCORRECT_DOCUMENT;
                    }
                }


                for (int row = 5; row <= sheet.LastRowNum; row++)
                {
                    IRow currentRow = sheet.GetRow(row);
                    if (currentRow != null)
                    {
                        //запускаем цикл по столбцам
                        for (int column = 0; column < 8; column++)
                        {
                            //получаем значение яейки
                            var stringCellValue = currentRow.GetCell(column);
                        }
                    }
                }
                return ErrorCodes.SUCCESS;
            }
            else
            {
                return ErrorCodes.INCORRECT_DOCUMENT;
            }
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
