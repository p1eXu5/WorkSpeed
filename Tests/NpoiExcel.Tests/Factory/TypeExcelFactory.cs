using System;

namespace NpoiExcel.Tests.Factory
{
    public class TypeExcelFactory
    {
        public static Type EmptyClass => typeof (Empty);
        public static Type ClassWithParameterizedCtor => typeof (ParameterizedCtor);

        class Empty { }

        class ParameterizedCtor
        {
            public ParameterizedCtor (int foo) { }
        }
    }
}
