using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Tests.ProductivityTests
{
    [ TestFixture ]
    public class ProductivityUnitTests
    {
        [SetUp]
        public void SetupCulture ()
        {
            CultureInfo.CurrentUICulture = new CultureInfo( "en-us" );
        }

        [ Test ]
        public void AddAction_ActionWithAnotherEmployee_Throws ()
        {
            // Arrange:
            var productivityEmployee = GetEmployee( "AR54321" );
            var productivity = GetProductivity( productivityEmployee );

            var duration = TimeSpan.FromSeconds( 5 );
            var actionEmployee = GetEmployee( "AR12345" );
            var action = GetGatheringAction( actionEmployee, OperationGroups.Gathering );

            // Action:
            // Assert:
            var ex = Assert.Catch<ArgumentException>( () => productivity.AddEmployeeAction( action ) );
        }

        [ Test ]
        public void AddAction_FistActionIsGathering_AddDuration ()
        {
            // Arrange:
            var employee = GetEmployee( "AR12345" );
            var productivity = GetProductivity( employee );
            var duration = TimeSpan.FromSeconds( 5 );
            var action = GetGatheringAction( employee, OperationGroups.Gathering, duration );

            // Action:
            productivity.AddEmployeeAction( action );

            // Assert:
            Assert.That( duration == productivity.GatheringTime, $"{productivity.GatheringTime}" );
        }


        #region Factory

        private Productivity GetProductivity ( Employee employee )
        {
            return new Productivity( employee );
        }

        private GatheringAction GetGatheringAction (
            Employee employee,
            OperationGroups group,
            TimeSpan duration = default( TimeSpan ),
            DateTime startTime = default( DateTime ),
            int productQuantity = 1,
            int id = 1
        )
        {
            var operationGroup = new OperationGroup { Id = id, Name = group };
            var operation = new Operation { Id = id, OperationGroupId = id, Group = operationGroup, Name = "Test operation" };

            var action = new GatheringAction {

                Id = id,
                Employee = employee,
                Operation = operation,
                StartTime = startTime,
                Duration = duration,
                ProductQuantity = productQuantity
            };

            return action;
        }

        private Employee GetEmployee (string id, bool isSmoker = false )
        {
            return new Employee { Id = id, Name = "TestEmployee", IsSmoker = isSmoker };
        }

        #endregion
    }
}
