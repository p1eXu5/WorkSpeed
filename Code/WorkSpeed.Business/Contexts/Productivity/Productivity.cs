using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public class Productivity : IProductivity
    {
        private readonly Dictionary< EmployeeActionBase, Period > _actions;

        public Productivity ()
        {
            _actions = new Dictionary< EmployeeActionBase, Period >();
        }

        public void Add ( EmployeeActionBase action, Period period )
        {
            _actions[ action ] = period;
        }

        public Period this [ EmployeeActionBase action ]
        {
            get => _actions[ action ];
            set => _actions[ action ] = value;
        }

        public TimeSpan  GetTime ()
        {
            return _actions.Values.Aggregate( TimeSpan.Zero, (acc, next) => acc + next.Duration );
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

        public IEnumerator< Period > GetEnumerator ()
        {
            return _actions.Values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator();
        }
    }
}
