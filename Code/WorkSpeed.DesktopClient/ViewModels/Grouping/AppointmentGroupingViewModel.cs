using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.ReportService;

namespace WorkSpeed.DesktopClient.ViewModels.Grouping
{
    public class AppointmentGroupingViewModel : FilteredViewModel
    {
        public AppointmentGroupingViewModel ( AppointmentGrouping appointmentGrouping, Predicate< object > predicate )
        {
            Appointment = appointmentGrouping.Appointment ?? throw new ArgumentNullException( nameof( appointmentGrouping ), @"AppointmentGrouping cannot be null." );

            CreateCollection();

            var view = SetupView( PositionGroupingVmCollection );
            view.SortDescriptions.Add( new SortDescription( "Position.Id", ListSortDirection.Ascending ) );


            void CreateCollection ()
            {
                var positionGroupingVmCollection = new ObservableCollection< PositionGroupingViewModel >( 
                    appointmentGrouping.PositionGrouping
                                       .Select( p =>
                                                {
                                                    var pgvm = new PositionGroupingViewModel( p, predicate );
                                                    pgvm.PropertyChanged += OnIsModifyChanged;
                                                    return pgvm;
                                                } ) 
                );
                PositionGroupingVmCollection = new ReadOnlyObservableCollection< PositionGroupingViewModel >( positionGroupingVmCollection );
            }
        }

        public Appointment Appointment { get;}
        public ReadOnlyObservableCollection< PositionGroupingViewModel > PositionGroupingVmCollection { get; private set; }

        public string Name => Appointment.InnerName;

        protected internal override void Refresh ()
        {
            foreach ( var position in PositionGroupingVmCollection ) {
                position.Refresh();
            }

            base.Refresh();
        }

        protected override void OnIsModifyChanged ( object sender, PropertyChangedEventArgs args )
        {
            base.OnIsModifyChanged( sender, args );

            IsModify = PositionGroupingVmCollection.Any( pgvm => pgvm.IsModify );
        }
    }
}
