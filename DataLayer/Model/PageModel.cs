﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    /// <summary>
    ///  Класс PageModel
    ///  модель для пагинации
    ///  при отображении данных
    /// </summary>
    /// 
    public class PageModel
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }

        public PageModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
