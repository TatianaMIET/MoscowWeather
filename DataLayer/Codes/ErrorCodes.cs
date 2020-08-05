using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Codes
{
    public sealed class ErrorCodes
    {
        public static int SUCCESS { get; } = 0;

        public static int INCORRECT_HEADER { get; } = 1;

        public static int PARSE_ERROR { get; } = 2;
    }
}

