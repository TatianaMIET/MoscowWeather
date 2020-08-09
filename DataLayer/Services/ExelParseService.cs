using DataLayer.Codes;
using DataLayer.Entity;
using DataLayer.Model;
using NLog;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace DataLayer.Services
{
    /// <summary>
    ///  Класс ExelParseService
    ///  сервис для 
    ///  выполнении парсинга Exel архивов
    /// </summary>
    public class ExelParseService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ExelParseResultModel ReadExelArchive(HttpPostedFileBase archive)
        {
            logger.Info("TRY PARSE  {0}", archive.FileName);

            ExelParseResultModel result = new ExelParseResultModel();
            List<Weather> weatherData = new List<Weather>();

            XSSFWorkbook xssfwb = new XSSFWorkbook(archive.InputStream);
            bool allDataIsCorrect = true;

            int sheetNum = xssfwb.NumberOfSheets;
            for (int num = 0; num < sheetNum; num++)
            {
                ISheet sheet = xssfwb.GetSheetAt(num);
                if (!HeaderCheck(sheet))
                {
                    logger.Error("Can't read sheet {0}", sheet.SheetName);
                    result.ErrorMessages.Add(string.Format(ErrorTextTemplates.INCORRECT_HEADER, archive.FileName));
                    break;
                }

                if (!ReadWeatherData(sheet, weatherData))
                {
                    allDataIsCorrect = false;
                }
            }
            if (!allDataIsCorrect)
            {
                result.ErrorMessages.Add(string.Format(ErrorTextTemplates.PARSE_ERROR, archive.FileName));
            }
            result.WeatherData = weatherData;
            return result;
        }

        private bool HeaderCheck(ISheet sheet)
        {
            Settings settings = new Settings();
            int firstHeaderRowNum = int.Parse(WebConfigurationManager.AppSettings["FirstHeaderRowNum"]);
            IRow firstHeadingRow = sheet.GetRow(firstHeaderRowNum);
            IRow secondHeadingRow = sheet.GetRow(++firstHeaderRowNum);

            if (firstHeadingRow.LastCellNum == secondHeadingRow.LastCellNum)
            {
                List<string> heading = new List<string>(firstHeadingRow.LastCellNum);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < firstHeadingRow.LastCellNum; i++)
                {
                    sb.Clear();
                    sb.Append(firstHeadingRow.GetCell(i));
                    sb.Append(" ");
                    sb.Append(secondHeadingRow.GetCell(i));
                    heading.Add(sb.ToString().Trim());
                }

                //Проверяем шапку на соответствие порядку и содержнию заголовков
                for (int i = 0; i < heading.Count; i++)
                {
                    if (!heading[i].Equals(settings.HeaderNames[i]))
                        return false;
                }
                return true;
            }
            return false;
        }

        private bool ReadWeatherData(ISheet sheet, List<Weather> result)
        {
            logger.Info("TRY PARSE SHEET {0}", sheet.SheetName);
            bool allDataIsCorrect = true;

            int firstDataRowNum = int.Parse(WebConfigurationManager.AppSettings["FirstDataRowNum"]);
            for (int row = firstDataRowNum; row < sheet.LastRowNum; row++)
            {
                IRow currentRow = sheet.GetRow(row);
                Weather weather = new Weather();
                StringBuilder sb = new StringBuilder();

                if (currentRow != null)
                {
                    logger.Info("TRY PARSE ROW {0}", row);
                    bool tryParse = true; int cellNum = 0;

                    decimal dec = 0M;
                    short sh = 0;
                    short? shNull = null;

                    while (tryParse && cellNum <= currentRow.LastCellNum)
                    {
                        switch (cellNum)
                        {
                            case 0:
                                DateTime dateTime = new DateTime();
                                tryParse = ParseDateTimeTypeCell(currentRow.GetCell(cellNum), currentRow.GetCell(++cellNum), ref dateTime);
                                if (tryParse)
                                {
                                    weather.Date = dateTime;
                                }
                                else
                                {
                                    logger.Error("Can't parse cells № {0} {1}", cellNum - 1, cellNum);
                                }
                                break;
                            case 2:
                                tryParse = ParseDecimalTypeCell(currentRow.GetCell(cellNum), ref dec);
                                if (tryParse)
                                {
                                    weather.Temperature = dec;
                                }
                                else
                                {
                                    logger.Error("Can't parse cell № {0}", cellNum);
                                }
                                break;
                            case 3:
                                tryParse = ParseDecimalTypeCell(currentRow.GetCell(cellNum), ref dec);
                                if (tryParse)
                                {
                                    weather.Humidity = dec;
                                }
                                else
                                {
                                    logger.Error("Can't parse cell № {0}", cellNum);
                                }
                                break;
                            case 4:
                                tryParse = ParseDecimalTypeCell(currentRow.GetCell(cellNum), ref dec);
                                if (tryParse)
                                {
                                    weather.DewPoint = dec;
                                }
                                else
                                {
                                    logger.Error("Can't parse cell № {0}", cellNum);
                                }
                                break;
                            case 5:
                                tryParse = ParseShortTypeCell(currentRow.GetCell(cellNum), ref sh);
                                if (tryParse)
                                {
                                    weather.Pressure = sh;
                                }
                                else
                                {
                                    logger.Error("Can't parse cell № {0}", cellNum);
                                }
                                break;
                            case 6:
                                sb.Clear();
                                tryParse = ParseStringNullableTypeCell(currentRow.GetCell(cellNum), sb);
                                if (tryParse)
                                {
                                    weather.WindDirection = sb.ToString();
                                }
                                else
                                {
                                    logger.Error("Can't parse cell № {0}", cellNum);
                                }
                                break;
                            case 7:
                                tryParse = ParseShortNullableTypeCell(currentRow.GetCell(cellNum), ref shNull);
                                if (tryParse)
                                {
                                    weather.WindSpeed = shNull;
                                }
                                else
                                {
                                    logger.Error("Can't parse cell № {0}", cellNum);
                                }
                                break;
                            case 8:
                                tryParse = ParseShortNullableTypeCell(currentRow.GetCell(cellNum), ref shNull);
                                if (tryParse)
                                {
                                    weather.Cloudiness = shNull;
                                }
                                else
                                {
                                    logger.Error("Can't parse cell № {0}", cellNum);
                                }
                                break;
                            case 9:
                                tryParse = ParseShortNullableTypeCell(currentRow.GetCell(cellNum), ref shNull);
                                if (tryParse)
                                {
                                    weather.CloudBase = shNull;
                                }
                                else
                                {
                                    logger.Error("Can't parse cell № {0}", cellNum);
                                }
                                break;
                            case 10:
                                tryParse = ParseShortNullableTypeCell(currentRow.GetCell(cellNum), ref shNull);
                                if (tryParse)
                                {
                                    weather.HorizontalVisibility = shNull;
                                }
                                else
                                {
                                    logger.Error("Can't parse cell № {0}", cellNum);
                                }
                                break;
                            case 11:
                                sb.Clear();
                                tryParse = ParseStringNullableTypeCell(currentRow.GetCell(cellNum), sb);
                                weather.WeatherPhenomena = sb?.ToString();
                                break;
                            default:
                                break;
                        }
                        cellNum++;
                    }
                    if (tryParse)
                    {
                        logger.Info("PARSE ROW {0}", row);
                        result.Add(weather);
                    }
                    else
                    {
                        allDataIsCorrect = false;
                    }
                }
            }
            logger.Info("PARSE SHEET {0}  COUNT {1}", sheet.SheetName, result.Count);
            return allDataIsCorrect;
        }

        private bool ParseDateTimeTypeCell(ICell dateCell, ICell timeCell, ref DateTime item)
        {
            if (dateCell == null || dateCell.ToString().Trim().Equals(""))
            {
                return false;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(dateCell.ToString());
                sb.Append(" ");

                if (timeCell == null || timeCell.ToString().Trim().Equals(""))
                {
                    return false;
                }
                else
                {
                    sb.Append(timeCell.ToString());
                    try
                    {
                        item = DateTime.Parse(sb.ToString());
                        return true;
                    }
                    catch (FormatException)
                    {
                        return false;
                    }
                }
            }
        }

        private bool ParseShortTypeCell(ICell currentCell, ref short item) => Parse<short>(currentCell, ref item);

        private bool ParseDecimalTypeCell(ICell currentCell, ref decimal item) => Parse<decimal>(currentCell, ref item);

        private bool ParseStringNullableTypeCell(ICell currentCell, StringBuilder item)
        {
            if (currentCell == null || currentCell.ToString().Trim().Equals(""))
            {
                item = null;
            }
            else
            {
                item.Append(currentCell.ToString().Trim());
            }
            return true;
        }

        private bool ParseShortNullableTypeCell(ICell currentCell, ref short? item)
        {
            if (currentCell == null || currentCell.ToString().Trim().Equals(""))
            {
                item = null;
                return true;
            }
            else
            {
                try
                {
                    item = short.Parse(currentCell.ToString().Trim());
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }
        }

        private bool Parse<T>(ICell currentCell, ref T item)
        {
            if (currentCell == null || currentCell.ToString().Trim().Equals(""))
            {
                return false;
            }
            else
            {
                try
                {
                    item = (T)Convert.ChangeType(currentCell.ToString(), typeof(T));
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                
            }
        }
    }
}

