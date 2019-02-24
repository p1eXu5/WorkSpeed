using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.Grouping;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public abstract class ReportViewModel : ViewModel, IReportViewModel
    {
        protected readonly IReportService _reportService;
        protected readonly IDialogRepository _dialogRepository;

        protected CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly ObservableCollection< ShiftViewModel > _shiftVmCollection;
        private readonly ObservableCollection< ShortBreakScheduleViewModel > _shortBreakCollection;
        

        private string _reportMessage;

        protected ReportViewModel ( IReportService reportService, IDialogRepository dialogRepository )
        {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService), @"ReportService cannot be null.");
            _dialogRepository = dialogRepository ?? throw new ArgumentNullException(nameof(dialogRepository), @"IDialogRepository cannot be null.");

            _shiftVmCollection = new ObservableCollection< ShiftViewModel >( _reportService.ShiftCollection.Select( sh => new ShiftViewModel( sh ) ) );
            ShiftVmCollection = new ReadOnlyObservableCollection< ShiftViewModel >( _shiftVmCollection );
            Observe( _reportService.ShiftCollection, _shiftVmCollection, sh => sh.Shift );

            _shortBreakCollection = new ObservableCollection< ShortBreakScheduleViewModel >( _reportService.ShortBreakCollection.Select( sbs => new ShortBreakScheduleViewModel( sbs ) ));
            ShortBreakVmCollection = new ReadOnlyObservableCollection< ShortBreakScheduleViewModel >( _shortBreakCollection );
            Observe( _reportService.ShortBreakCollection, _shortBreakCollection, sbs => sbs.ShortBreakSchedule  );

            AppointmentVmCollection = _reportService.AppointmentCollection.Select( a => new AppointmentViewModel( a ) ).ToArray();
            RankVmCollection = _reportService.RankCollection.Select( r => new RankViewModel( r ) );
            PositionVmCollection = _reportService.PositionCollection.Select( p => new PositionViewModel( p ) ).ToArray();
        }

        public ReadOnlyObservableCollection< ShiftViewModel > ShiftVmCollection { get; }
        public ReadOnlyObservableCollection< ShortBreakScheduleViewModel > ShortBreakVmCollection { get; }

        

        public IEnumerable< AppointmentViewModel > AppointmentVmCollection { get; }
        public IEnumerable< PositionViewModel > PositionVmCollection { get; }
        public IEnumerable< RankViewModel > RankVmCollection { get; }

        public string ReportMessage
        {
            get => _reportMessage;
            set {
                _reportMessage = value;
                OnPropertyChanged();
            }
        }

        public abstract void OnSelectedAsync ();
    }
}
