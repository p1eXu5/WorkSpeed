using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.ActionDetails;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Business.Tests.Contexts.Productivity.UnitTests
{
    [ TestFixture ]
    public class ProductivityBuilderTests
    {

        #region CheckDuration

        [ Test, Category( "CheckDuration" ) ]
        public void CheckDuration__DoubleAddressAction_GatheringGroup__AddsNotZeroPeriodToDictionary ()
        {
            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Test Operation", Group = OperationGroups.Gathering };

            var action = new DoubleAddressAction {
                StartTime = DateTime.Parse( "22.02.2019 8:00:45" ),
                Duration = TimeSpan.FromSeconds( 20 ),
                Operation = operation
            };

            // Action:
            builder.CheckDuration( action );

            // Assert:
            var res = builder.GetResult().Item1[ operation ].GetTime();
            Assert.That( res, Is.Not.EqualTo( Period.Zero ) );
        }

        [ Test, Category( "CheckDuration" ) ]
        public void CheckDuration__DoubleAddressAction_OperationIsNull__Throws ()
        {
            // Arrange:
            var builder = GetBuilder();

            var action = new DoubleAddressAction {
                StartTime = DateTime.Parse( "22.02.2019 8:00:45" ),
                Duration = TimeSpan.FromSeconds( 20 ),
            };

            // Action:
            // Assert:
            Assert.Catch<ArgumentException>( () => builder.CheckDuration( action ));
        }

        [ TestCase(5, 5) ]
        [ TestCase(100, 100), Category( "CheckDuration" ) ]
        public void CheckDuration__DoubleAddressAction_PackingGroup_DurationLessProductOfQuantityByK__AddsPeriodGreaterOperationDuration ( int duration, int quantity )
        {
            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Test Operation", Group = OperationGroups.Packing };

            var action = new DoubleAddressAction {
                StartTime = DateTime.Parse( "22.02.2019 8:00:45" ),
                Duration = TimeSpan.FromSeconds( duration ),
                Operation = operation,
                DoubleAddressDetails = new List< DoubleAddressActionDetail >( new [] {
                    new DoubleAddressActionDetail { ProductQuantity = quantity }, 
                })
            };

            // Action:
            builder.CheckDuration( action );

            // Assert:
            var res = builder.GetResult().Item1[ operation ].GetTime().TotalSeconds;
            Assert.That( res, Is.GreaterThan( duration ) );
        }

        [ TestCase(50, 5) ]
        [ TestCase(1001, 100), Category( "CheckDuration" ) ]
        public void CheckDuration__DoubleAddressAction_PackingGroup_DurationGreaterProductOfQuantityByK__AddsPeriodGreaterOperationDuration ( int duration, int quantity )
        {
            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Test Operation", Group = OperationGroups.Packing };

            var action = new DoubleAddressAction {
                StartTime = DateTime.Parse( "22.02.2019 8:00:45" ),
                Duration = TimeSpan.FromSeconds( duration ),
                Operation = operation,
                DoubleAddressDetails = new List< DoubleAddressActionDetail >( new [] {
                    new DoubleAddressActionDetail { ProductQuantity = quantity }, 
                })
            };

            // Action:
            builder.CheckDuration( action );

            // Assert:
            var res = builder.GetResult().Item1[ operation ].GetTime().TotalSeconds;
            Assert.That( res, Is.EqualTo( duration ) );
        }

        [ Test, Category( "CheckDuration" ) ]
        public void CheckDuration__DoubleAddressAction_BuyerGatheringGroup_VolumeLess5__AddsPeriodGreaterBy30Sec ()
        {
            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Test Operation", Group = OperationGroups.BuyerGathering };
            var duration = 1;

            var action = new DoubleAddressAction {
                StartTime = DateTime.Parse( "22.02.2019 8:00:45" ),
                Duration = TimeSpan.FromSeconds( duration ),
                Operation = operation,
                DoubleAddressDetails = new List< DoubleAddressActionDetail >( new [] {
                    new DoubleAddressActionDetail { ProductQuantity = 1, Product = new Product { ItemVolume = 1 }}, 
                    new DoubleAddressActionDetail { ProductQuantity = 1, Product = new Product { ItemVolume = 3 }}, 
                })
            };

            // Action:
            builder.CheckDuration( action );

            // Assert:
            var res = builder.GetResult().Item1[ operation ].GetTime().TotalSeconds;
            Assert.That( res, Is.EqualTo( duration + 30 ) );
        }

        [ Test, Category( "CheckDuration" ) ]
        public void CheckDuration__DoubleAddressAction_BuyerGatheringGroup_VolumeGreater5__AddsPeriodGreaterBy60Sec ()
        {
            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Test Operation", Group = OperationGroups.BuyerGathering };
            var duration = 1;

            var action = new DoubleAddressAction {
                StartTime = DateTime.Parse( "22.02.2019 8:00:45" ),
                Duration = TimeSpan.FromSeconds( duration ),
                Operation = operation,
                DoubleAddressDetails = new List< DoubleAddressActionDetail >( new [] {
                    new DoubleAddressActionDetail { ProductQuantity = 1, Product = new Product { ItemVolume = 11 }}, 
                    new DoubleAddressActionDetail { ProductQuantity = 1, Product = new Product { ItemVolume = 3 }}, 
                })
            };

            // Action:
            builder.CheckDuration( action );

            // Assert:
            var res = builder.GetResult().Item1[ operation ].GetTime().TotalSeconds;
            Assert.That( res, Is.EqualTo( duration + 60 ) );
        }

        [ Test, Category( "CheckDuration" ) ]
        public void CheckDuration__DoubleAddressAction_ReseptionGroup__AddsPeriodGreaterBy10Sec ()
        {
            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Test Operation", Group = OperationGroups.Reception };
            var duration = 1;

            var action = new ReceptionAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:00:45" ),
                Duration = TimeSpan.FromSeconds( duration ),
                Operation = operation,
                ReceptionActionDetails = new List< ReceptionActionDetail >( new [] {
                    new ReceptionActionDetail { ProductQuantity = 1, Product = new Product { ItemVolume = 11 }}, 
                    new ReceptionActionDetail { ProductQuantity = 1, Product = new Product { ItemVolume = 3 }}, 
                })
            };

            // Action:
            builder.CheckDuration( action );

            // Assert:
            var res = builder.GetResult().Item1[ operation ].GetTime().TotalSeconds;
            Assert.That( res, Is.EqualTo( duration + 10 ) );
        }

        #endregion


        #region CheckPause

        [ Test, Category( "CheckPause" ) ]
        public void CheckPause__BothNotPacking_PauseLessThresholdOr20__PauseIsNotAdded ()
        {
            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Test Operation", Group = OperationGroups.Gathering };

            var currentAction = new ReceptionAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:00:00" ),
                Duration = TimeSpan.FromSeconds( 30 ),
                Operation = operation,
            };

            var nextAction = new ReceptionAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:00:45" ),
                Duration = TimeSpan.FromSeconds( 15 ),
                Operation = operation,
            };

            var next = builder.CheckDuration( nextAction );
            var current = builder.CheckDuration( currentAction );

            // Action:
            builder.CheckPause( current, next );

            // Assert:
            var res = builder.GetResult().downtimes.FirstOrDefault();
            Assert.That( res, Is.EqualTo( Period.Zero ) );
        }

        [ Test, Category( "CheckPause" ) ]
        public void CheckPause__BothNotPacking_PauseGreaterThresholdOr20__AddsPause ()
        {
            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Test Operation", Group = OperationGroups.Gathering };

            var currentAction = new ReceptionAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:00:00" ),
                Duration = TimeSpan.FromSeconds( 30 ),
                Operation = operation,
            };

            var nextAction = new ReceptionAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:10:45" ),
                Duration = TimeSpan.FromSeconds( 15 ),
                Operation = operation,
            };

            var next = builder.CheckDuration( nextAction );
            var current = builder.CheckDuration( currentAction );

            // Action:
            builder.CheckPause( current, next );

            // Assert:
            var res = builder.GetResult().downtimes.FirstOrDefault();
            Assert.That( res, Is.Not.EqualTo( Period.Zero ) );
        }

        [ Test, Category( "CheckPause" ) ]
        public void CheckPause__BothNotPacking_CurrentEndGreaterNextStart__ReduseCurrentDuration ()
        {
            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Test Operation", Group = OperationGroups.Gathering };
            var originalCurrentDuration = 1000; // ~16 min.

            var currentAction = new ReceptionAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:00:00" ),
                Duration = TimeSpan.FromSeconds( originalCurrentDuration ),
                Operation = operation,
            };

            var nextAction = new ReceptionAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:10:45" ),
                Duration = TimeSpan.FromSeconds( 15 ),
                Operation = operation,
            };

            var next = builder.CheckDuration( nextAction );
            var current = builder.CheckDuration( currentAction );

            // Action:
            builder.CheckPause( current, next );

            // Assert:
            var res = builder.GetResult().productivityMap[ operation ][currentAction];
            Assert.That( res.Duration.TotalSeconds, Is.LessThan( originalCurrentDuration ) );
        }


        #endregion


        #region Factory

        private ProductivityBuilder GetBuilder ()
        {
            return new ProductivityBuilder();
        }

        #endregion
    }
}
