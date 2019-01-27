//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WorkSpeed.FileModels;

//namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
//{
//    public class ActionsImportStageViewModel : ImportStageViewModel
//    {
//        public ActionsImportStageViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum )
//            : base( fastProductivityViewModel, stageNum )
//        {
//            IsInProgress = false;
//        }

//        public override string Header { get; } = "Действия сотрудников. Импорт.";

//        protected override async void Open ( object obj )
//        {
//            var fileName = base.OpenExcelFile();

//            if ( String.IsNullOrWhiteSpace( fileName ) ) return;

//            FileName = fileName;

//            IsInProgress = true;
//            ProgressCounter = 0;
//            Message = "Загрузка...";

//            var actionsLastCount = Warehouse.GetGatheringActions().Count();

//            var cancellationToken = GetCancellationToken();

//            bool areActionsAdded = await Warehouse.ImportAsync< GatheringImportModel >( fileName, cancellationToken, Progress );

//            if ( areActionsAdded )
//            {

//                Message = $"Добавлено { Warehouse.GetGatheringActions().Count() - actionsLastCount } действий сотрудников";
//                UpdateCanForward();
//            }
//            else
//            {

//                IsInProgress = false;
//                Message = "Файл не содержит действий сотрудников.";
//            }
//        }

//        protected override bool CanForward ( object obj )
//        {
//            return Warehouse.GetGatheringActions().Any();
//        }
//    }
//}
