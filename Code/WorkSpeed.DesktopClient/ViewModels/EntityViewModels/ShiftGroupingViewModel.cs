﻿using System;
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

namespace WorkSpeed.DesktopClient.ViewModels.EntityViewModels
{
    public class ShiftGroupingViewModel : ViewModel
    {
        private readonly ObservableCollection< AppointmentGroupingViewModel > _appointments;

        public ShiftGroupingViewModel ( ShiftGrouping shiftGrouping )
        {
            Shift = shiftGrouping.Shift ?? throw new ArgumentNullException(nameof(shiftGrouping), @"ShiftGrouping cannot be null."); ;
            _appointments = new ObservableCollection< AppointmentGroupingViewModel >( shiftGrouping.Appointments.Select( a => new AppointmentGroupingViewModel( a ) ) );
            Appointments = new ReadOnlyObservableCollection< AppointmentGroupingViewModel >( _appointments );

            var view = CollectionViewSource.GetDefaultView( Appointments );
            view.SortDescriptions.Add( new SortDescription( "Appointment.Id", ListSortDirection.Ascending ) );
        }

        public Shift Shift { get; }
        public ReadOnlyObservableCollection< AppointmentGroupingViewModel > Appointments { get; }

        public string Name => Shift.Name;
    }
}
