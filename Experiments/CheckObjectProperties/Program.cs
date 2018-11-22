using System;
using System.Reflection;

namespace CheckObjectProperties
{
    class Program
    {
        static void Main(string[] args)
        {
            Type type = typeof(SomeClass);

            foreach (PropertyInfo propertyInfo in type.GetProperties()) {

                Console.WriteLine (propertyInfo.Attributes);
                Console.WriteLine (propertyInfo.CanWrite);
                Console.WriteLine (propertyInfo.Name);
                Console.WriteLine ("\n");
            }

            Console.ReadKey (true);
        }
    }

    class SomeClass
    {
        public int Num { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }

        protected int I1 { get; set; }
        public virtual int I2 { get; set; }
        internal int I3 { get; set; }
        private int I4 { get; set; }
    }

    enum ReadingTypes
    {
        Int,
        Float,
        String,
        DateTime,
        Time,
        Symbol,
        Key,
    }
}
