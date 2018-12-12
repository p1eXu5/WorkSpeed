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
        private readonly List< QuantityIndicators > _indicatorsCollection;

        public CompositeQuantityIndicators ( string name,  ICategoryConstraints constraints )
            :base(name, constraints)
        {
            _indicatorsCollection = new List< QuantityIndicators >();
        }

        public CompositeQuantityIndicators ( string name,  ICategoryConstraints constraints,  IEnumerable< QuantityIndicators > list )
            : base( name, constraints )
        {
            _indicatorsCollection = new List< QuantityIndicators >( list );
        }

        public void AddIndicators ( QuantityIndicators indicators ) => _indicatorsCollection.Add( indicators );

        public void RemoveIndicators ( QuantityIndicators indicators )
        {
            if ( _indicatorsCollection.Contains( indicators ) ) {
                _indicatorsCollection.Remove( indicators );
            }
        }

        void IQuantityIndicators.AddQuantity ( EmployeeAction gatheringAction )
        {
            foreach ( var quantityIndicators in _indicatorsCollection ) {
                
                ( ( IQuantityIndicators )quantityIndicators ).AddQuantity( gatheringAction );
            }

            ;
        }

        public override string GetName ()
        {
            throw new NotImplementedException();
        }
    }
}
