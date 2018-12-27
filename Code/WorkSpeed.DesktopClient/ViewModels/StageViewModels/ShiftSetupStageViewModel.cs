using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    public class ShiftSetupStageViewModel : StageViewModel
    {
        public ShiftSetupStageViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum )
            : base( fastProductivityViewModel, stageNum )
        {
            Shifts = new ObservableCollection< Shift >( Warehouse.GetShifts() );

            CanMoveNext = true;
        }

        public override string Header { get; } = "Настройка смен";

        public ObservableCollection< Shift > Shifts { get; set; }

        protected override void Forward ( object obj )
        {
            Warehouse.UpdateShiftBreaks();

            base.Forward( obj );
        }
    }
}
