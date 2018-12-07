using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelImporter.Tests.Factory
{
    internal enum Types
    {
        WithoutParameterlessCtor = -1,
        Default,
        Normal,
        Headerless,
        WithHideAttributes,
        WithCombinedAttributes,
        MultyHeadered
    }

    internal class TypeFactory
    {
        private static TypeFactory _instance;

        private TypeFactory() { }

        public static TypeFactory Instance => _instance ?? (_instance = new TypeFactory()); 

        public Type this[Types index]
        {
            get {
                switch (index) {
                    case Types.WithoutParameterlessCtor:
                        return typeof (IncorrectType);
                    case Types.Default:
                        return typeof (DefaultType);
                    case Types.Normal:
                        return typeof (NormalType);
                    case Types.Headerless:
                        return typeof (HeaderlessType);
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public IEnumerable<string> GetHeaders (Types testType)
        {
            throw new NotFiniteNumberException();
        }

        public IEnumerable<string> GetWritableProperties (Types testType)
        {
            throw new NotFiniteNumberException();
        }

        private class IncorrectType
        {
            public IncorrectType (int a) { }
        }

        private class DefaultType
        {
        }

        private class NormalType
        {
        }

        private class HeaderlessType
        {
        }
    }
}
