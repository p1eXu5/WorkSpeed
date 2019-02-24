using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.Grouping;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class ProductivityReportViewModel : ReportViewModel
    {
        public ProductivityReportViewModel ( IReportService reportService, IDialogRepository dialogRepository ) 
            : base( reportService, dialogRepository ) 
        { }

        public override void OnSelectedAsync ()
        {
            throw new NotImplementedException();
        }
    }
}
