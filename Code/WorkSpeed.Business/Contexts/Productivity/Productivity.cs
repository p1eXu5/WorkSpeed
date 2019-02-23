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
        private readonly Dictionary< string, Period > _actions;

        public Productivity ()
        {
            _actions = new Dictionary< string, Period >();
        }

        public void Add ( EmployeeActionBase action, Period period )
        {
            _actions[ action.Id ] = period;
        }

        public Period this [ EmployeeActionBase action ]
        {
            get => _actions[ action.Id ];
            set => _actions[ action.Id ] = value;
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
    }
}
