using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.Entities
{
    public abstract class AbbreviationViewModel : ViewModel
    {
        public string Abbreviation { get; protected set; }

        protected string GetAbbreviation ( string abbreviations )
        {
            StringBuilder sb = new StringBuilder();

            using ( var enumerator = abbreviations.GetEnumerator() ) {

                while ( enumerator.MoveNext() ) {
                    if ( enumerator.Current == ';' ) { break; }
                    sb.Append( enumerator.Current );
                }
            }

            return sb.ToString();
        }
    }
}
