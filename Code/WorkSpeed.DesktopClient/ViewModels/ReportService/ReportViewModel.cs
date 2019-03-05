using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Entities;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService
{
    public abstract class ReportViewModel : FilteredViewModel
    {
        #region Fields
        

        protected readonly ObservableCollection< FilterViewModel > _filterVmCollection;

        protected readonly IReportService _reportService;
        protected readonly IDialogRepository _dialogRepository;

        protected CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private string _reportMessage;

        #endregion


        #region Ctor

        protected ReportViewModel ( IReportService reportService, IDialogRepository dialogRepository )
        {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService), @"ReportService cannot be null.");
            _dialogRepository = dialogRepository ?? throw new ArgumentNullException(nameof(dialogRepository), @"IDialogRepository cannot be null.");

            CreateCommonCollections();
            _filterVmCollection = GetFilterCollection();
            FilterVmCollection = new ReadOnlyObservableCollection< FilterViewModel >( _filterVmCollection );


            void CreateCommonCollections ()
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
        }

        #endregion


        #region Properties

        public string ReportMessage
        {
            get => _reportMessage;
            set {
                _reportMessage = value;
                OnPropertyChanged();
            }
        }
        
        public ReadOnlyObservableCollection< FilterViewModel > FilterVmCollection { get; }
        
        public ReadOnlyObservableCollection< AppointmentViewModel > AppointmentVmCollection { get; private set; }
        public ReadOnlyObservableCollection< PositionViewModel > PositionVmCollection { get; private set; }
        public ReadOnlyObservableCollection< RankViewModel > RankVmCollection { get; private set; }
        public ReadOnlyObservableCollection< ShiftViewModel > ShiftVmCollection { get; private set; }
        public ReadOnlyObservableCollection< ShortBreakScheduleViewModel > ShortBreakVmCollection { get; private set; }

        #endregion


        #region Methods

        public abstract Task UpdateAsync ();

        protected ObservableCollection< FilterViewModel > GetFilterCollection ()
        {
            var coll = new ObservableCollection< FilterViewModel>( new[] {
                new FilterViewModel( "Работает", FilterIndexes.IsActive, true ),
                new FilterViewModel( "Зоны ответственности", FilterIndexes.Position, PositionVmCollection.Select( p => p.Position ), p => (( Position )p).Name ),
                new FilterViewModel( "Должности", FilterIndexes.Appointment, AppointmentVmCollection.Select( a => a.Appointment ), a => (( Appointment )a).InnerName ),
                new FilterViewModel( "Смены", FilterIndexes.Shift, ShiftVmCollection.Select( s => s.Shift ), s => (( Shift )s).Name ),
                new FilterViewModel( "Ранги", FilterIndexes.Rank, RankVmCollection.Select( r => r.Rank ), r => (( Rank )r).Number.ToString( CultureInfo.InvariantCulture ) ),
                new FilterViewModel( "Курит", FilterIndexes.IsSmoker, new object[] {
                                                                                        new Tuple<bool?,string> ( true, "Да" ), 
                                                                                        new Tuple<bool?,string> (false, "Нет"), 
                                                                                        new Tuple<bool?,string> (null, "Не известно")
                                                                                    }, b => ((Tuple<bool?,string>)b).Item2 ),
            });

            coll[ (int)FilterIndexes.IsActive ].FilterChanged += OnPredicateChange;
            coll[ (int)FilterIndexes.Position ].FilterChanged += OnPredicateChange;
            coll[ (int)FilterIndexes.Appointment ].FilterChanged += OnPredicateChange;
            coll[ (int)FilterIndexes.Shift ].FilterChanged += OnPredicateChange;
            coll[ (int)FilterIndexes.Rank ].FilterChanged += OnPredicateChange;
            coll[ (int)FilterIndexes.IsSmoker ].FilterChanged += OnPredicateChange;

            return coll;
        }

        protected virtual void OnPredicateChange( object sender, FilterChangedEventArgs args ) => Refresh();

        protected bool IsActivePredicate ( EmployeeViewModel employee )
            => _filterVmCollection[ (int)FilterIndexes.IsActive ].Entities.Any( obj => (obj).Equals( employee.IsActive ) );

        protected bool PositionPredicate ( EmployeeViewModel employee )
            => _filterVmCollection[ (int)FilterIndexes.Position ].Entities.Any( obj => (obj as Position) == employee.Position);

        protected bool AppointmentPredicate ( EmployeeViewModel employee )
            => _filterVmCollection[ (int)FilterIndexes.Appointment ].Entities.Any( obj => (obj as Appointment) == employee.Appointment);

        protected bool ShiftPredicate ( EmployeeViewModel employee )
            => _filterVmCollection[ (int)FilterIndexes.Shift ].Entities.Any( obj => (obj as Shift) == employee.Shift);

        protected bool RankPredicate ( EmployeeViewModel employee )
            => _filterVmCollection[ (int)FilterIndexes.Rank ].Entities.Any( obj => (( Rank )obj).Number == employee.Rank.Number);

        protected bool IsSmokerPredicate ( EmployeeViewModel employee )
            => _filterVmCollection[ (int)FilterIndexes.IsSmoker ].Entities.Any( obj => ((Tuple<bool?,string>)obj).Item1.Equals( employee.IsSmoker ) );

        #endregion
    }
}
