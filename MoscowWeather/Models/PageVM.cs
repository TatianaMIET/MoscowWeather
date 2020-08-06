using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoscowWeather.Models
{

    /// <summary>
    /// Класс PageVM
    /// модель для реализации отображения
    /// с помощью пагинации
    /// </summary>
    public class PageVM
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }

      

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }
    }
}