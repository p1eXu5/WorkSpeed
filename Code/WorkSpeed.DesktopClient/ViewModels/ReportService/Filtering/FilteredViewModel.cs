using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Contracts;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering
{
    /// <summary>
    ///     Base class for view models that have ability for contained collections filtering.
    ///     (Implements <see cref="ICollectionViewList"/>)
    /// </summary>
    public abstract class FilteredViewModel : ViewModel
    {


        protected internal abstract void Refresh ( FilterIndexes filter );
    }
}
