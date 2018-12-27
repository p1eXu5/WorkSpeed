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




        #region Factory


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
