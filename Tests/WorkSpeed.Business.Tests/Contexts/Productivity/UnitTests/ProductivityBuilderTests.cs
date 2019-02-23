using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WorkSpeed.Business.Contexts.Productivity.Builders;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Tests.Contexts.Productivity.UnitTests
{
    [ TestFixture ]
    public class ProductivityBuilderTests
    {
        [ Test ]
        public void CheckDuration_GatheringOperationGroup_AddsNotZeroPeriodToDictionary ()
        {
            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Test Operation" };

            var action = new DoubleAddressAction {
                StartTime = DateTime.Parse( "22.02.2019 8:00:45" ),
                Duration = TimeSpan.FromSeconds( 20 ),
                Operation = operation
            };

            // Action:
            builder.CheckDuration( action );

            // Assert:
            var res = builder.Productivities[ operation ].GetTime();
            Assert.That( res, Is.Not.EqualTo( Period.Zero ) );
        }


        #region Factory

        private ProductivityBuilder GetBuilder ()
        {
            return new ProductivityBuilder();
        }

        #endregion
    }
}
