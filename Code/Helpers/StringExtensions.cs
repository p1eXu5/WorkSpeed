using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Helpers
{
    public static class StringExtensions
    {
        public static string RemoveWhitespaces (this string str)
        {
            return str.ToCharArray().Where (c => !Char.IsWhiteSpace (c)).Aggregate ("", (s, c1) => s += c1);
        }

        public static string ToProperty (this string str)
        {
            var chars = str.ToCharArray().Where(Char.IsLetterOrDigit).ToArray();
            if (!chars.Any()) throw new ArgumentException();

            var i = 0;

            while (Char.IsDigit(chars[i]) && i < chars.Length) {
                ++i;
            }

            if (i == chars.Length) throw new ArgumentException();

            chars[0] = Char.ToUpper (chars[0]);
                
            return chars.Aggregate("", (s, c1) => s += c1);
        }

        public static bool HasWhitespaces(this string str)
        {
            return str.ToCharArray().Where(Char.IsWhiteSpace).Count() != 0;
        }

        public static string AppendAssemblyPath(this string fileName, string subpath = "")
        {
            return new StringBuilder().Append( Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location ) )
                                      .Append( subpath.ConvertToDirName() )
                                      .Append ("\\")
                                      .Append(fileName).ToString();
        }

        public static string ConvertToDirName(this string dirName)
        {
            return dirName.ToCharArray().Where(c => !Char.IsWhiteSpace(c) && IsValidForDirName(c)).Aggregate("", (s, c1) => s += c1);

            bool IsValidForDirName (char c)
            {
                switch (c) {

                    case ':':
                    case '*':
                    case '?':
                    case '"':
                    case '<':
                    case '>':
                    case '|':
                        return false;
                    default:
                        return true;
                }
            }
        }

        public static Type GetPrimitiveType (this string primitiveType)
        {
            if (String.IsNullOrWhiteSpace (primitiveType)) throw new ArgumentException();

            switch (primitiveType) {
                case "bool":
                    return typeof (bool);
                case "int":
                    return typeof (int);
                case "short":
                    return typeof (short);
                case "byte":
                    return typeof (byte);
                case "double":
                    return typeof (double);
                case "string":
                    return typeof (string);
                case "DateTime":
                    return typeof (DateTime);
                default:
                    return typeof (object);
            }
        }
    }
}
