using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WorkSpeed.Import
{
    public static class StringHelper
    {
        public static string RemoveWhitespaces (this string str)
        {
            return str.ToCharArray().Where (c => !Char.IsWhiteSpace (c)).Aggregate ("", (s, c1) => s += c1);
        }

        public static bool HasWhitespaces(this string str)
        {
            return str.ToCharArray().Where(Char.IsWhiteSpace).Count() != 0;
        }

        /// <summary>
        /// In 6 times slower than RemoveWhitespaces()
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveWhitespacesWithRegex (this string str)
        {
            return Regex.Replace (str, @"\s+", String.Empty);
        }
    }
}
