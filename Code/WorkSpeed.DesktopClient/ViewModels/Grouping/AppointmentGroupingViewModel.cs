using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.ReportService;

namespace WorkSpeed.DesktopClient.ViewModels.Grouping
{
    public class AppointmentGroupingViewModel : FilteredViewModel
    {
        private readonly ObservableCollection< PositionGroupingViewModel > _positions;

        public AppointmentGroupingViewModel ( AppointmentGrouping appointmentGrouping, Predicate< object > predicate )
        {
            Appointment = appointmentGrouping.Appointment ?? throw new ArgumentNullException( nameof( appointmentGrouping ), @"AppointmentGrouping cannot be null." );

            _positions = new ObservableCollection< PositionGroupingViewModel >( 
                appointmentGrouping.PositionGrouping
                                    .Select( p => new PositionGroupingViewModel( p, predicate ) ) 
            );
            Positions = new ReadOnlyObservableCollection< PositionGroupingViewModel >( _positions );

            ViewList = CollectionViewSource.GetDefaultView( Positions );
            ViewList.SortDescriptions.Add( new SortDescription( "Position.Id", ListSortDirection.Ascending ) );

            ViewList.Filter = Predicate;
        }

        public Appointment Appointment { get;}
        public ReadOnlyObservableCollection< PositionGroupingViewModel > Positions { get; }

        public string Name => Appointment.InnerName;

        protected override void OnRefresh ()
        {
            foreach ( var position in _positions ) {
                position.Refresh();
            }

            base.OnRefresh();
        }
    }
}
