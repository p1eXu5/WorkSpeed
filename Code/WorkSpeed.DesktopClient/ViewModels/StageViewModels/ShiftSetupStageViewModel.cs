//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using WorkSpeed.Data.Models;
//using WorkSpeed.DesktopClient.ViewModels.EntityViewModels;
//using WorkSpeed.MvvmBaseLibrary;

//namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
//{
//    public class ShiftSetupStageViewModel : StageViewModel
//    {
//        #region Ctor

//        public ShiftSetupStageViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum )
//            : base( fastProductivityViewModel, stageNum )
//        {
//            Shifts = new ObservableCollection< Shift >( Warehouse.GetShifts() );
//            BreakList = new ObservableCollection< ShortBreakViewModel >( Warehouse.GetBreakList().Select( (sb) => new ShortBreakViewModel( sb ) ) );
//            SwitchIsForSmokerCommand = new MvvmCommand( SwitchIsForSmoker );

//            CanMoveNext = true;
//        }

//        #endregion


//        #region Properties 

//        public override string Header { get; } = "Настройка смен";

//        public ObservableCollection< Shift > Shifts { get; set; }
//        public ObservableCollection< ShortBreakViewModel > BreakList { get; set; }

//        public ICommand SwitchIsForSmokerCommand { get; }
        
//        #endregion

//        protected override void Forward ( object obj )
//        {
//            Warehouse.UpdateShiftBreaks();

//            Warehouse.UpdateFixedBreaks();

//            base.Forward( obj );
//        }

//        private void SwitchIsForSmoker ( object obj )
//        {
//            if ( !(obj is ShortBreakViewModel shortBreak)) return;

//            if ( shortBreak.IsForSmokers ) {

//                foreach ( var other in BreakList.Where( b => b.Id != shortBreak.Id ) ) {
//                    other.IsForSmokers = false;
//                }
//            }
//            else {
//                BreakList.Last().IsForSmokers = true;
//            }
//        }

//    }
//}
