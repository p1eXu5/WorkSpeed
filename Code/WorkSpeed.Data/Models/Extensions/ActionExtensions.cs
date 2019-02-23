using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models.ActionDetails;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Data.Models.Extensions
{
    public static class ActionExtensions
    {
        public static TimeSpan GetPlacingDuration ( this DoubleAddressAction action, double[] koefs )
        {
            if ( action.Operation == null 
                 || action.Operation.Group != OperationGroups.Placing
                 || action.DoubleAddressDetails == null
                 || action.DoubleAddressDetails.Count == 0 
                 || koefs == null
                 || koefs.Length < 4 ) return action.Duration;

            var quantity = action.DoubleAddressDetails.Sum( d => d.ProductQuantity );
            var dur = action.Duration.Seconds;
            int s;

            if ( quantity < 10 ) { return GetDuration( 0 ); }
            if ( quantity < 20 ) { return GetDuration( 1 ); }
            if ( quantity < 30 ) { return GetDuration( 2 ); }
            return GetDuration( 3 );

            TimeSpan GetDuration ( int i )
            {
                checked { s = (int)Math.Floor(quantity * koefs[ i ]); }
                return dur < s 
                    ? TimeSpan.FromSeconds( s )
                    : action.Duration;
            }
        }

        public static TimeSpan GetBuyerGatheringDuration ( this DoubleAddressAction action, double bound, int min, int max )
        {
            if ( action.Operation == null
                 || action.Operation.Group != OperationGroups.BuyerGathering
                 || action.DoubleAddressDetails == null
                 || action.DoubleAddressDetails.Count == 0 ) return action.Duration;

            var volume = action.DoubleAddressDetails.Sum( d => d.Volume() );
            
            return volume < bound 
                       ? action.Duration + TimeSpan.FromSeconds( min )
                       : action.Duration + TimeSpan.FromSeconds( max );
        }
    }
}
