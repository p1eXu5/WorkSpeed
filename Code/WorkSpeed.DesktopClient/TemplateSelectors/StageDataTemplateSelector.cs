//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using WorkSpeed.DesktopClient.ViewModels.StageViewModels;

//namespace WorkSpeed.DesktopClient.TemplateSelectors
//{
//    public class StageDataTemplateSelector : DataTemplateSelector
//    {
//        public override DataTemplate SelectTemplate ( object item, DependencyObject container )
//        {
//            if ( container is FrameworkElement element ) {

//                if ( item is ShiftSetupStageViewModel )
//                {
//                    var o = element.FindResource( "dt_ShiftSetup" ) as DataTemplate;
//                    return o;
//                }

//                if ( item is CategoriesThresholdStageViewModel )
//                {
//                    var o = element.FindResource( "dt_CategoriesThreshold" ) as DataTemplate;
//                    return o;
//                }

//                if ( item is ImportStageViewModel ) {
//                    var o =  element.FindResource( "dt_ImportFile" ) as DataTemplate;
//                    return o;
//                }

//                if ( item is CheckEmployeesStageViewModel )
//                {
//                    var o = element.FindResource( "dt_EmployeeTable" ) as DataTemplate;
//                    return o;
//                }

//                if ( item is ProductivityStageViewModel )
//                {
//                    var o = element.FindResource( "dt_ProductivityTable" ) as DataTemplate;
//                    return o;
//                }
//            }

//            return null;
//        }
//    }
//}
