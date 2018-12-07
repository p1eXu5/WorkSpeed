using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ExcelImporter.Tests.Factory
{
    public class TypeExcelFactory
    {
        private const string _subdir = "TestFiles";
        private const string _fileName = "test";

        public static Type EmptyClass => typeof (Empty);
        public static Type ClassWithParameterizedCtor => typeof (ParameterizedCtor);

        public static IEnumerable TestCases()
        {
            // 1
            var sheet = new[,]
            {
                { "", "", "", "", "" },
                { "", "", "", "", "" },
                { "", "", "", "", "" },
                { "", "", "", "", "" },
            };

            var type = new[,]
            {
                { "", "", "", "", "" },
                { "", "", "", "", "" },
                { "", "", "", "", "" },
                { "", "", "", "", "" },
            };

            yield return new TestCaseData(sheet)
                .SetName($"Test case #1;");
        }

        private 

        class Empty { }

        class ParameterizedCtor
        {
            public ParameterizedCtor (int foo) { }
        }
    }
}
