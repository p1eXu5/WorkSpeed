using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Experiments.Wpf.ViewModels;

namespace Experiments.Wpf
{
    public class MainViewModel
    {
        public object Model { get; set; } = new ListBoxItemsSourceDynamicChangeViewModel();
    }
}
