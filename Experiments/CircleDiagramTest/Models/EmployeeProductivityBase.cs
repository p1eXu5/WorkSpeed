﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleDiagramTest.Models
{
    public class EmployeeProductivityBase
    {
        public static List< EmployeeProductivityBase > EmployeeProductivities { get; }

        static EmployeeProductivityBase ()
        {
            EmployeeProductivities = new List< EmployeeProductivityBase >();

            EmployeeProductivities.Add( new EmployeeProductivityBase( new Employee { Id = "AR00001", Name = "Employee 1", Rank = 3 } ) );

            EmployeeProductivities[ 0 ][Operation.Operations[0]] = new GatheringProductivity();
            EmployeeProductivities[ 0 ][Operation.Operations[1]] = new ReceptionProductivity();
        }




        private readonly Dictionary< Operation, IProductivity > _productivities;

        public EmployeeProductivityBase ( Employee employee )
        {
            Employee = employee;
            _productivities = new Dictionary< Operation, IProductivity >();
        }

        public Employee Employee { get; set; }

        public IProductivity this[ Operation operation ]
        {
            get => _productivities[ operation ];
            set => _productivities[ operation ] = value;
        }

        public void AddRange ( IEnumerable< IProductivity > productivities )
        {
            foreach ( var productivity in productivities ) {

                _productivities[ productivity.Operation ] = productivity;
            }
        }
    }
}