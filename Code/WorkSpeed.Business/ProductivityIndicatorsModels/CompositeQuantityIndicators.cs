﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Constraints;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.ProductivityIndicatorsModels
{
    public class CompositeQuantityIndicators : QuantityIndicators
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

        protected override void OnChangeCategoryConstraints ( ICategoryConstraints categoryConstraints )
        {
            foreach ( var indicators in _indicatorsDictionary.Values ) {

                indicators.CategoryConstraints = categoryConstraints;
            }
        }

        protected override void Add ( EmployeeActionBase employeeAction )
        {
            foreach ( var indicators in _indicatorsDictionary.Values ) {

                indicators.AddQuantity( employeeAction );
            }
        }

        public QuantityIndicators this [ string key ] => _indicatorsDictionary[ key ];

    }
}
