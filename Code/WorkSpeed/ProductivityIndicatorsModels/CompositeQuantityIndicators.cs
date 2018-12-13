using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.Interfaces;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public class CompositeQuantityIndicators : QuantityIndicators, IQuantityIndicators
    {
        private readonly Dictionary< string, QuantityIndicators > _indicatorsDictionary;

        public CompositeQuantityIndicators ( string name )
            : base( name )
        {
        }

        public CompositeQuantityIndicators ( string name,  ICategoryConstraints constraints )
            :base(name, constraints)
        {
            _indicatorsDictionary = new Dictionary< string, QuantityIndicators >();
        }

        public CompositeQuantityIndicators ( string name,  ICategoryConstraints constraints,  IEnumerable< QuantityIndicators > indicatorsList )
            : this( name, constraints )
        {
            var indicators = indicatorsList.ToArray();
            foreach (var quantityIndicators in indicators) {
                _indicatorsDictionary[  quantityIndicators.Name ] = quantityIndicators;
            }
        }

        public void AddIndicators ( QuantityIndicators indicators )
        {
            indicators.CategoryConstraints = _categoryConstraints;
            _indicatorsDictionary[ indicators.Name ] = indicators;
        } 

        public void RemoveIndicators ( QuantityIndicators indicators )
        {
            if ( _indicatorsDictionary.ContainsKey( indicators.Name ) ) {
                _indicatorsDictionary.Remove( indicators.Name );
            }
        }

        void IQuantityIndicators.AddQuantity ( EmployeeAction gatheringAction )
        {
            foreach ( var indicators in _indicatorsDictionary.Values ) {
                
                ( ( IQuantityIndicators )indicators ).AddQuantity( gatheringAction );
            }
        }

        public QuantityIndicators this [ string key ] => _indicatorsDictionary[ key ];

    }
}
