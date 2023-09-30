using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lst = HolidaysLib.GetGreekHolidays(DateTime.Now.Year, true, true);

            foreach (var item in lst)
            {
                Console.WriteLine(item.Key.ToString("dd-MM-yyyy") + ": " + item.Value);
            }
            Console.ReadKey();
        }
    }
}
