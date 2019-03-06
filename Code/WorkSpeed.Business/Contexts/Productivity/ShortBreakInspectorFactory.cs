using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public class ShortBreakInspectorFactory : IShortBreakInspectorFactory
    {
        private readonly Dictionary< ShortBreakSchedule, IShortBreakInspector > _map;
        private readonly object _locker = new object();

        public ShortBreakInspectorFactory ()
        {
            _map = new Dictionary< ShortBreakSchedule, IShortBreakInspector >();
        }

        public IShortBreakInspector GetShortBreakInspector ( ShortBreakSchedule shortBreaks )
        {
            if ( !_map.ContainsKey( shortBreaks ) ) {
                lock ( _locker ) {
                    _map[ shortBreaks ] = new ShortBreakInspector( shortBreaks );
                }
            }
            return _map[ shortBreaks ];
        }
    }
}
