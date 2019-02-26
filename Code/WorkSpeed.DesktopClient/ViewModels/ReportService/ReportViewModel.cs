using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.Grouping;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService
{
    public abstract class ReportViewModel : FilteredViewModel, IReportViewModel
    {
        protected const int IS_ACTIVE = 0;
        protected const int POSITION = 1;
        protected const int APPOINTMENT = 2;
        protected const int SHIFT = 3;
        protected const int RANK = 4;
        protected const int IS_SMOKER = 5;

        protected const int COUNT = 5;


        protected readonly ObservableCollection< EntityFilterViewModel< object > > _filterVmCollection;

        protected readonly IReportService _reportService;
        protected readonly IDialogRepository _dialogRepository;

        protected CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();


        private string _reportMessage;


        #region Ctor

        protected ReportViewModel ( IReportService reportService, IDialogRepository dialogRepository )
        {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService), @"ReportService cannot be null.");
            _dialogRepository = dialogRepository ?? throw new ArgumentNullException(nameof(dialogRepository), @"IDialogRepository cannot be null.");

            CreateCommonCollections();
            _filterVmCollection = GetFilterCollection();
            FilterVmCollection = new ReadOnlyObservableCollection< EntityFilterViewModel< object > >( _filterVmCollection );
        }


        private void CreateCommonCollections ()
        {
            var appointmentVmCollection = new ObservableCollection< AppointmentViewModel >( _reportService.AppointmentCollection.Select( a => new AppointmentViewModel( a ) ) );
            AppointmentVmCollection = new ReadOnlyObservableCollection< AppointmentViewModel >( appointmentVmCollection );
            Observe( _reportService.AppointmentCollection, appointmentVmCollection, a => a.Appointment );

            var positionVmCollection = new ObservableCollection< PositionViewModel >( _reportService.PositionCollection.Select( p => new PositionViewModel( p ) ) );
            PositionVmCollection = new ReadOnlyObservableCollection< PositionViewModel >( positionVmCollection );
            Observe( _reportService.PositionCollection, positionVmCollection, p => p.Position );

            var rankVmCollection = new ObservableCollection< RankViewModel >( _reportService.RankCollection.Select( r => new RankViewModel( r ) ) );
            RankVmCollection = new ReadOnlyObservableCollection< RankViewModel >( rankVmCollection );
            Observe( _reportService.RankCollection, rankVmCollection, r => r.Rank );

            var shiftVmCollection = new ObservableCollection< ShiftViewModel >( _reportService.ShiftCollection.Select( sh => new ShiftViewModel( sh ) ) );
            ShiftVmCollection = new ReadOnlyObservableCollection< ShiftViewModel >( shiftVmCollection );
            Observe( _reportService.ShiftCollection, shiftVmCollection, sh => sh.Shift );

            var shortBreakCollection = new ObservableCollection< ShortBreakScheduleViewModel >( _reportService.ShortBreakCollection.Select( sbs => new ShortBreakScheduleViewModel( sbs ) ) );
            ShortBreakVmCollection = new ReadOnlyObservableCollection< ShortBreakScheduleViewModel >( shortBreakCollection );
            Observe( _reportService.ShortBreakCollection, shortBreakCollection, sbs => sbs.ShortBreakSchedule );
        }

        #endregion

        public ReadOnlyObservableCollection< AppointmentViewModel > AppointmentVmCollection { get; private set; }
        public ReadOnlyObservableCollection< PositionViewModel > PositionVmCollection { get; private set; }
        public ReadOnlyObservableCollection< RankViewModel > RankVmCollection { get; private set; }
        public ReadOnlyObservableCollection< ShiftViewModel > ShiftVmCollection { get; private set; }
        public ReadOnlyObservableCollection< ShortBreakScheduleViewModel > ShortBreakVmCollection { get; private set; }

        public ReadOnlyObservableCollection< EntityFilterViewModel< object > > FilterVmCollection { get; }

        public string ReportMessage
        {
            get => _reportMessage;
            set {
                _reportMessage = value;
                OnPropertyChanged();
            }
        }

        public abstract void OnSelectedAsync ();

        

        protected ObservableCollection< EntityFilterViewModel< object > > GetFilterCollection ()
        {
            var coll = new ObservableCollection< EntityFilterViewModel< object > >( new[] {
                new EntityFilterViewModel< object >( "Работает", new[] { ( object )true, ( object )false }, b => "да" ),
                new EntityFilterViewModel< object >( "Зоны ответственности", PositionVmCollection, p => (( PositionViewModel )p).Name ),
                new EntityFilterViewModel< object >( "Должности", AppointmentVmCollection, a => (( AppointmentViewModel )a).InnerName ),
                new EntityFilterViewModel< object >( "Смены", ShiftVmCollection, s => (( ShiftViewModel )s).Name ),
                new EntityFilterViewModel< object >( "Ранги", RankVmCollection, r => (( RankViewModel )r).Number.ToString( CultureInfo.InvariantCulture ) ),
                new EntityFilterViewModel< object >( "Работает", new[] { ( object )true, ( object )false }, b => "да" ),
            });

            ((INotifyCollectionChanged)coll[ IS_ACTIVE ].Entities).CollectionChanged += OnPredicateChange;
            ((INotifyCollectionChanged)coll[ POSITION ].Entities).CollectionChanged += OnPredicateChange;
            ((INotifyCollectionChanged)coll[ APPOINTMENT ].Entities).CollectionChanged += OnPredicateChange;
            ((INotifyCollectionChanged)coll[ SHIFT ].Entities).CollectionChanged += OnPredicateChange;
            ((INotifyCollectionChanged)coll[ RANK ].Entities).CollectionChanged += OnPredicateChange;
            ((INotifyCollectionChanged)coll[ IS_SMOKER ].Entities).CollectionChanged += OnPredicateChange;

            return coll;
        }

        protected virtual void OnPredicateChange ( object sender, EventArgs args )
        {
            Refresh();
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        protected bool EmployeePredicate(object o)
        {
            if (!(o is EmployeeViewModel employee)) return false;

            return _filterVmCollection[IS_ACTIVE].Entities.Any(obj => (obj is bool) == employee.IsActive)
                   && _filterVmCollection[POSITION].Entities.Any(obj => (obj as PositionViewModel).Position == employee.Position)
                   && _filterVmCollection[APPOINTMENT].Entities.Any(obj => (obj as AppointmentViewModel).Appointment == employee.Appointment)
                   && _filterVmCollection[SHIFT].Entities.Any(obj => (obj as ShiftViewModel).Shift == employee.Shift)
                   && _filterVmCollection[RANK].Entities.Any(obj => (obj as RankViewModel).Number == employee.Rank.Number)
                   && _filterVmCollection[IS_SMOKER].Entities.Any(obj => (obj is bool) == employee.IsSmoker);
        }
    }
}
