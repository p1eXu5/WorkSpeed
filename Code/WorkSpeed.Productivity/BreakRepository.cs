using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public class BreakRepository : IBreakRepository
    {


        public TimeSpan GetLongest ( Period period )
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetShortest ( Employee employee )
        {
            throw new NotImplementedException();
        }

        public TimeSpan CheckFixed ( Period period, Employee employee )
        {
            throw new NotImplementedException();
        }

        public void SetVariableBreak ( string name, TimeSpan duration, Period period )
        {

        }

        public void SetFixedBreaks ( string name, 
                                     TimeSpan duration, 
                                     TimeSpan interval, TimeSpan offset, 
                                     Predicate<Employee> predicate )
        {

        }
    }
}
