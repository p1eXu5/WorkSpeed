using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CircleDiagram
{
    public class CircleDiagramControl : ItemsControl
    {
        static CircleDiagramControl ()
        {
            DefaultStyleKeyProperty.OverrideMetadata( typeof( CircleDiagramControl ), new FrameworkPropertyMetadata( typeof( CircleDiagramControl ) ) );
        }
        protected override void OnItemsSourceChanged ( IEnumerable oldValue, IEnumerable newValue )
        {
            base.OnItemsSourceChanged( oldValue, newValue );
        }
    }
}
