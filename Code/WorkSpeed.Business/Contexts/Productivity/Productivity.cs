using System;
using System.Collections.Generic;
using System.Linq;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public class Productivity : IProductivity
    {
        private readonly Dictionary< Period, EmployeeActionBase > _actions;

        public Productivity ()
        {
            _actions = new Dictionary< Period, EmployeeActionBase >();
        }

        public void Add ( Period period, EmployeeActionBase action )
        {
            _actions[ period ] = action;
        }

        public TimeSpan  GetTime ()
        {
            return _actions.Keys.Aggregate( TimeSpan.Zero, (acc, next) => acc + next.Duration );
        }


        public double GetLinesPerHour ()
        {
            throw new NotImplementedException();
        }

        public double GetScansPerHour ()
        {
            throw new NotImplementedException();
        }

        public IEnumerable< (int, Category) > GetLines ( IEnumerable< Category > categories )
        {
            throw new NotImplementedException();
        }

        public IEnumerable< (int, Category) > GetScans ( IEnumerable< Category > categories )
        {
            throw new NotImplementedException();
        }

        public IEnumerable< int > GetQuantity ()
        {
            throw new NotImplementedException();
        }

        public IEnumerable< double > GetVolumes ()
        {
            throw new NotImplementedException();
        }

        public IEnumerable< double > GetWeigths ()
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetTotalTime ()
        {
            throw new NotImplementedException();
        }
    }
}
