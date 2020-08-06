using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Codes
{
    public class ErrorTextTemplates
    {
        public static string SUCCESS { get; } = "Документ {0} успешно загружен! ";

        public static string INCORRECT_HEADER { get; } = "Не все листы в документе {0} были успешно загружены ";

        public static string PARSE_ERROR { get; } = "Данные документа {0} были считаны не полностью ";

        public static string FILE_TYPE_ERROR { get; } = "Некорректный тип документа {0} ";
    }
}
