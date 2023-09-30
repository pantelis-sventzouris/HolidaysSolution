using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsLib
{
    public sealed class StringHelper
    {
        public static string GetUnicodeString(string sourceText)
        {
            byte[] bytes = Encoding.GetEncoding("windows-1253").GetBytes(sourceText);
            return Encoding.GetEncoding("windows-1253").GetString(bytes);
        }

    }
}
