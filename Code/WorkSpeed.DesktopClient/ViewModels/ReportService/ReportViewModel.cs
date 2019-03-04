using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Contexts.Contracts;
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
                new FilterViewModel( "Работает", true ),
                new FilterViewModel( "Зоны ответственности", PositionVmCollection, p => (( PositionViewModel )p).Name ),
                new FilterViewModel( "Должности", AppointmentVmCollection, a => (( AppointmentViewModel )a).InnerName ),
                new FilterViewModel( "Смены", ShiftVmCollection, s => (( ShiftViewModel )s).Name ),
                new FilterViewModel( "Ранги", RankVmCollection, r => (( RankViewModel )r).Number.ToString( CultureInfo.InvariantCulture ) ),
                new FilterViewModel( "Курит", null ),
            });

            coll[ (int)FilterIndexes.IsActive ].FilterChanged += OnPredicateChange;
            coll[ (int)FilterIndexes.Position ].FilterChanged += OnPredicateChange;
            coll[ (int)FilterIndexes.Appointment ].FilterChanged += OnPredicateChange;
            coll[ (int)FilterIndexes.Shift ].FilterChanged += OnPredicateChange;
            coll[ (int)FilterIndexes.Rank ].FilterChanged += OnPredicateChange;
            coll[ (int)FilterIndexes.IsSmoker ].FilterChanged += OnPredicateChange;

            return coll;
        }

        protected virtual void OnPredicateChange ( object sender, EventArgs args )
        {
            Refresh();
        }

        #endregion
    }
}
