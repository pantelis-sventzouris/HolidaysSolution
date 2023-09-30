using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidaysLib
{
    public sealed class HolidaysLib
    {
        public static List<KeyValuePair<DateTime, string>> GetGreekHolidays(int year, bool includeCatholicHolidays = false, bool includeWeekends = false)
        {
            List<KeyValuePair<DateTime, string>> greekHolidays = new List<KeyValuePair<DateTime, string>>();

            greekHolidays.AddRange(GetStandardGreekHolidays(year));
            greekHolidays.AddRange(GetOrthodoxEasterHolidays(year));
            if (includeCatholicHolidays)
            {
                greekHolidays.AddRange(GetCatholicEasterHolidays(year));
            }
            if (includeWeekends)
            {
                greekHolidays.AddRange(GetAllWeekends(year));
            }

            List<KeyValuePair<DateTime, string>> sortedHolidays = greekHolidays.OrderBy(x => x.Key.Date).Distinct().ToList();
            return greekHolidays;
        }

        public static List<KeyValuePair<DateTime, string>> GetStandardGreekHolidays(int year)
        {
            List<KeyValuePair<DateTime, string>> standardHolidays = new List<KeyValuePair<DateTime, string>>();
            standardHolidays.Add(new KeyValuePair<DateTime, string>(new DateTime(year, 1, 1), "Πρωτοχρονιά"));
            standardHolidays.Add(new KeyValuePair<DateTime, string>(new DateTime(year, 1, 6), "Θεοφάνεια"));
            standardHolidays.Add(new KeyValuePair<DateTime, string>(new DateTime(year, 3, 25), "25η Μαρτίου"));
            standardHolidays.Add(new KeyValuePair<DateTime, string>(new DateTime(year, 8, 15), "Δεκαπενταύγουστος"));
            standardHolidays.Add(new KeyValuePair<DateTime, string>(new DateTime(year, 10, 28), "28η Οκτωβρίου"));
            standardHolidays.Add(new KeyValuePair<DateTime, string>(new DateTime(year, 12, 25), "Χριστούγεννα"));
            standardHolidays.Add(new KeyValuePair<DateTime, string>(new DateTime(year, 12, 26), "2η μέρα Χριστουγέννων"));
            return standardHolidays;
        }

        public static List<KeyValuePair<DateTime, string>> GetOrthodoxEasterHolidays(int year)
        {
            List<KeyValuePair<DateTime, string>> orthodoxEasterHolidays = new List<KeyValuePair<DateTime, string>>();
            DateTime orthodoxEasterDay = GetOrthodoxEaster(year);
            orthodoxEasterHolidays.Add(new KeyValuePair<DateTime, string>(orthodoxEasterDay, "Ορθόδοξο Πάσχα"));
            orthodoxEasterHolidays.Add(new KeyValuePair<DateTime, string>(orthodoxEasterDay.AddDays(-48), "Καθαρά Δευτέρα"));
            orthodoxEasterHolidays.Add(new KeyValuePair<DateTime, string>(orthodoxEasterDay.AddDays(-2), "Μεγάλη Παρασκευή (Ορθ. Πάσχα)"));
            orthodoxEasterHolidays.Add(new KeyValuePair<DateTime, string>(orthodoxEasterDay.AddDays(-1), "Μεγάλο Σάβαβτο (Ορθ. Πάσχα)"));
            orthodoxEasterHolidays.Add(new KeyValuePair<DateTime, string>(orthodoxEasterDay.AddDays(1), "Δευτέρα Πάσχα (Ορθ. Πάσχα)"));
            orthodoxEasterHolidays.Add(new KeyValuePair<DateTime, string>(orthodoxEasterDay.AddDays(50), "Αγ. Πνεύματος"));
            return orthodoxEasterHolidays;
        }

        private static DateTime GetOrthodoxEaster(int year)
        {
            int a = year % 19;
            int b = year % 7;
            int c = year % 4;
            int d = (19 * a + 16) % 30;
            int e = (2 * c + 4 * b + 6 * d) % 7;
            int f = (19 * a + 16) % 30;
            int key = f + e + 3;
            int month = (key > 30) ? 5 : 4;
            int day = (key > 30) ? key - 30 : key;
            return new DateTime(year, month, day);
        }

        private static DateTime GetCatholicEaster(int year)
        {
            int month = 3;
            int a = year % 19 + 1;
            int b = year / 100 + 1;
            int c = (3 * b) / 4 - 12;
            int d = (8 * b + 5) / 25 - 5;
            int e = (5 * year) / 4 - c - 10;
            int f = (11 * a + 20 + d - c) % 30;
            if (f == 24) { f++; }
            if ((f == 25) && (a > 11)) { f++; }
            var g = 44 - f;
            if (g < 21) { g += 30; }
            var day = (g + 7) - ((e + g) % 7);
            if (day > 31)
            {
                day -= 31;
                month = 4;
            }
            return new DateTime(year, month, day);
        }

        public static List<KeyValuePair<DateTime, string>> GetCatholicEasterHolidays(int year)
        {
            List<KeyValuePair<DateTime, string>> catholicEasterHolidays = new List<KeyValuePair<DateTime, string>>();
            DateTime catholicEasterDay = GetCatholicEaster(year);
            catholicEasterHolidays.Add(new KeyValuePair<DateTime, string>(catholicEasterDay, "Καθολικό Πάσχα"));
            catholicEasterHolidays.Add(new KeyValuePair<DateTime, string>(catholicEasterDay.AddDays(-2), "Μεγάλη Παρασκευή (Καθ. Πάσχα)"));
            catholicEasterHolidays.Add(new KeyValuePair<DateTime, string>(catholicEasterDay.AddDays(-1), "Μεγάλο Σάβαβτο (Καθ. Πάσχα)"));
            catholicEasterHolidays.Add(new KeyValuePair<DateTime, string>(catholicEasterDay.AddDays(1), "Δευτέρα Πάσχα (Καθ. Πάσχα)"));
            return catholicEasterHolidays;
        }

        public static List<KeyValuePair<DateTime, string>> GetAllWeekends(int year)
        {
            List<KeyValuePair<DateTime, string>> weekends = new List<KeyValuePair<DateTime, string>>();
            IEnumerable<DateTime> allDaysOfYear = GetAllDaysOfYear(year);

            foreach (DateTime day in allDaysOfYear)
            {
                if (day.DayOfWeek == DayOfWeek.Saturday)
                {
                    weekends.Add(new KeyValuePair<DateTime, string>(day, "Σάββατο"));
                }
                if (day.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekends.Add(new KeyValuePair<DateTime, string>(day, "Κυριακή"));
                }
            }
            return weekends;
        }

        private static IEnumerable<DateTime> GetAllDaysOfYear(int year)
        {
            DateTime start = new DateTime(year, 1, 1);
            DateTime end = new DateTime(year, 12, 31);

            for (DateTime i = start; i <= end; i = i.AddDays(1))
            {
                yield return i;
            }
        }

    }
}
