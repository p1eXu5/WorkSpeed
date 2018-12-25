using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity.ActionDetails
{
    public class TimeActionDetails
    {
        /// <summary>
        /// Null check.
        /// </summary>
        /// <param name="action"><see cref="EmployeeAction"/></param>
        /// <param name="pause">Pause between actions</param>
        public virtual void AddDetails ( EmployeeAction action, TimeSpan pause )
        {
            if ( action == null ) throw new ArgumentNullException( nameof( action ), "EmployeeActiuon cannot be null." );

            End = action.StartTime.Add( action.Duration );

            if ( Duration == TimeSpan.Zero ) {

                Start = action.StartTime;
                Duration = action.Duration;
            }
            else {
                Duration += pause + action.Duration;
            }
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
