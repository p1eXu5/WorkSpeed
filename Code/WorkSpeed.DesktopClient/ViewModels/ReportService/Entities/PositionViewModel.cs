using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Entities
{
    public class PositionViewModel : AbbreviationViewModel
    {
        private readonly Position _position;

        public PositionViewModel ( Position position )
        {
            _position = position;
            Abbreviation = GetAbbreviation( _position.Abbreviations );
        }

        public Position Position => _position;
        public string Name => _position.Name;
    }
}
