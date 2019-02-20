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

namespace WorkSpeed.DesktopClient.ViewModels.Grouping
{
    public class AppointmentGroupingViewModel : ViewModel
    {
        private readonly ObservableCollection< PositionGroupingViewModel > _positions;

        public AppointmentGroupingViewModel ( AppointmentGrouping appointmentGrouping )
        {
            Appointment = appointmentGrouping.Appointment ?? throw new ArgumentNullException( nameof( appointmentGrouping ), @"AppointmentGrouping cannot be null." );
            _positions = new ObservableCollection< PositionGroupingViewModel >( appointmentGrouping.PositionGrouping.Select( p => new PositionGroupingViewModel( p ) ) );
            Positions = new ReadOnlyObservableCollection< PositionGroupingViewModel >( _positions );

            var view = CollectionViewSource.GetDefaultView( Positions );
            view.SortDescriptions.Add( new SortDescription( "Position.Id", ListSortDirection.Ascending ) );
        }

        public Appointment Appointment { get;}
        public ReadOnlyObservableCollection< PositionGroupingViewModel > Positions { get; }

        public string Name => Appointment.InnerName;
    }
}
