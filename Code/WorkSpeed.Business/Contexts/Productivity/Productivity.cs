using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.ActionDetails;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Productivity
{
    /// <summary>
    ///     Created for each operation.
    /// </summary>
    public class Productivity : IProductivity
    {
        private readonly Dictionary< EmployeeActionBase, Period > _actions;

        #region Ctor

        public Productivity ()
        {
            _actions = new Dictionary< EmployeeActionBase, Period >();
        }

        #endregion


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
            if ( _actions.Count == 0 ) { return 0.0; }

            var lines = _actions.Keys.Where( a => a );
        }

        public double GetScansPerHour ()
        {
            throw new NotImplementedException();
        }

        public double GetTotalVolume ()
        {
            throw new NotImplementedException();
        }

        public (double client, double nonClient) GetCargoQuantity ()
        {
            throw new NotImplementedException();
        }

        public IEnumerable< (int count, Category category) > GetLines ( IEnumerable< Category > categories )
        {
            throw new NotImplementedException();
        }

        public IEnumerable< (int count, Category category) > GetScans ( IEnumerable< Category > categories )
        {
            throw new NotImplementedException();
        }

        public IEnumerable< (int count, Category category) > GetQuantity ( IEnumerable< Category > categories )
        {
            throw new NotImplementedException();
        }

        public IEnumerable< (double count, Category category) > GetVolumes ( IEnumerable< Category > categories )
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
