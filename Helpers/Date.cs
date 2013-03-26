using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class Date
    {
        public static string FormatDateLong(DateTime date)
        {
            return MonthName(date.Month) + " " + date.Day.ToString() + ", " + date.Year;
        }

        public static string MonthName(int month)
        {
            if (month == 1)
                return "January";
            else if (month == 2)
                return "February";
            else if (month == 3)
                return "March";
            else if (month == 4)
                return "April";
            else if (month == 5)
                return "May";
            else if (month == 6)
                return "June";
            else if (month == 7)
                return "July";
            else if (month == 8)
                return "August";
            else if (month == 9)
                return "September";
            else if (month == 10)
                return "October";
            else if (month == 11)
                return "November";
            else
                return "December";
        }
    }
}
