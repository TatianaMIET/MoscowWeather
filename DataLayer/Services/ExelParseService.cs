using DataLayer.Codes;
using DataLayer.Entity;
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


        public Dictionary<string, object> ReadExelArchive(HttpPostedFileBase archive)
        {
            logger.Info("TRY PARSE  {0}", archive.FileName);
            Dictionary<string, object> result = new Dictionary<string, object>();
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
                    result.Add("header_error", ErrorCodes.INCORRECT_HEADER);
                    break;
                }

                if (!ReadWeatherData(sheet, weatherData))
                {
                    allDataIsCorrect = false;
                }
            }
            if (!allDataIsCorrect)
            {
                result.Add("parse_error", ErrorCodes.PARSE_ERROR);
            }
            result.Add("result", weatherData);
            return result;
        }

        private bool HeaderCheck(ISheet sheet)
        {
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
                bool isCorrect = true;
                for (int i = 0; i < heading.Count; i++)
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
                            isCorrect = false;
                            break;
                    }

                    if (!isCorrect)
                    {
                        return isCorrect;
                    }
                }
                return isCorrect;
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
                    logger.Info("TRY PARSE ROW {0}" + row);
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
                                weather.Date = dateTime;
                                break;
                            case 2:
                                tryParse = ParseDecimalTypeCell(currentRow.GetCell(cellNum), ref dec);
                                weather.Temperature = dec;
                                break;
                            case 3:
                                tryParse = ParseDecimalTypeCell(currentRow.GetCell(cellNum), ref dec);
                                weather.Humidity = sh;
                                break;
                            case 4:
                                tryParse = ParseDecimalTypeCell(currentRow.GetCell(cellNum), ref dec);
                                weather.DewPoint = dec;
                                break;
                            case 5:
                                tryParse = ParseShortTypeCell(currentRow.GetCell(cellNum), ref sh);
                                weather.Pressure = sh;
                                break;
                            case 6:
                                sb.Clear();
                                tryParse = ParseStringNullableTypeCell(currentRow.GetCell(cellNum), sb);
                                weather.WindDirection = sb.ToString();
                                break;
                            case 7:
                                tryParse = ParseShortNullableTypeCell(currentRow.GetCell(cellNum), ref shNull);
                                weather.WindSpeed = sh;
                                break;
                            case 8:
                                tryParse = ParseShortNullableTypeCell(currentRow.GetCell(cellNum), ref shNull);
                                weather.Cloudiness = shNull;
                                break;
                            case 9:
                                tryParse = ParseShortNullableTypeCell(currentRow.GetCell(cellNum), ref shNull);
                                weather.CloudBase = shNull;
                                break;
                            case 10:
                                tryParse = ParseShortNullableTypeCell(currentRow.GetCell(cellNum), ref shNull);
                                weather.HorizontalVisibility = shNull;
                                break;
                            case 11:
                                sb.Clear();
                                tryParse = ParseStringNullableTypeCell(currentRow.GetCell(cellNum), sb);
                                weather.WeatherPhenomena =  sb?.ToString();
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
            if (dateCell == null)
            {
                logger.Error("Can't parse cell № {0}", dateCell.Address);
                return false;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(dateCell.ToString());
                sb.Append(" ");

                if (timeCell == null)
                {
                    logger.Error("Can't parse cell № {0}", timeCell.Address);
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
                        logger.Error("Can't parse cell № {0} {1}", dateCell.Address, timeCell.Address);
                        return false;
                    }
                }
            }
        }

        private bool ParseShortTypeCell(ICell currentCell, ref short item)
        {
            if (currentCell == null)
            {
                logger.Error("Can't parse cell № " + currentCell.Address);
                return false;
            }
            else
            {
                try
                {
                    item = short.Parse(currentCell.ToString());
                    return true;
                }
                catch (FormatException)
                {
                    logger.Error("Can't parse cell № {0}", currentCell.Address);
                    return false;
                }
            }
        }

        private bool ParseDecimalTypeCell(ICell currentCell, ref decimal item)
        {
            if (currentCell == null)
            {
                logger.Error("Can't parse cell № {0}", currentCell.Address);
                return false;
            }
            else
            {
                try
                {
                    item = decimal.Parse(currentCell.ToString());
                    return true;
                }
                catch (FormatException)
                {
                    logger.Error("Can't parse cell № {0}", currentCell.Address);
                    return false;
                }
            }
        }

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
                    logger.Error("Can't parse cell № {0}", currentCell.Address);
                    return false;
                }
            }
        }
    }
}


