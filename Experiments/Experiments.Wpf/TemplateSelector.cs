using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Experiments.Wpf.ViewModels;

namespace Experiments.Wpf
{
    public class TemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate ( object item, DependencyObject container )
        {
            if ( container is FrameworkElement elem ) {

                switch ( item ) {
                    case ListBoxItemsSourceDynamicChangeViewModel vm :
                        return elem.FindResource( "ListBoxItemsSourceDynamicChangeViewModel" ) as DataTemplate;
                }
            }

            return null;
        }
    }
}
