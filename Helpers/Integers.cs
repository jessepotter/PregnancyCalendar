using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class Integers
    {
        public static string Ordinal(int integer)
        {
            if (integer == 1)
                return "first";
            else if (integer == 2)
                return "second";
            else if (integer == 3)
                return "third";
            else if (integer == 4)
                return "fourth";
            else if (integer == 5)
                return "fifth";
            else if (integer == 6)
                return "sixth";
            else if (integer == 7)
                return "seventh";
            else if (integer == 8)
                return "eighth";
            else
                return "";
        }

        public static string OrdinalTwoLetter(int integer)
        {
            string name = Ordinal(integer);
            if (name == "")
                return "";
            return name.Substring(name.Length - 2, 2);
        }
    }
}
