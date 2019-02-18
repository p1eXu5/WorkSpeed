using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Models
{
    [ Serializable ]
    public class OperationThresholds
    {
        private readonly Dictionary< int, ushort > _thresholds;

        public OperationThresholds ()
        {
            _thresholds = new Dictionary< int, ushort >();
        }

        [ field:NonSerialized ]
        public event EventHandler< EventArgs > ThresholdChanged; 

        public ushort this [ Operation operation ]
        {
            get => _thresholds[ operation.Id ];
            set {
                _thresholds[ operation.Id ] = value;
                OnThresholdChanged();
            }
        }

        private void OnThresholdChanged ()
        {
            ThresholdChanged?.Invoke( this, new EventArgs() );
        }
    }
}
