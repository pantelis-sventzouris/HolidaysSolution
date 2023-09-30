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
            string targetText = String.Empty;

            byte[] bytes = Encoding.GetEncoding("windows-1253").GetBytes(sourceText);
            targetText = Encoding.GetEncoding("windows-1253").GetString(bytes);

            return targetText;

            //return sourceText;
        }

    }
}
