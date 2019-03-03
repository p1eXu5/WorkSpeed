using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Business.Models;
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

        [ Test, Category( "CheckDuration" ) ]
        public void CheckDuration__DoubleAddressAction_PackingGroup_DetailsIsNull__AddsPeriodEqualsDuration ()
        {
            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Test Operation", Group = OperationGroups.Packing };
            int duration = 10;

            var action = new DoubleAddressAction {
                StartTime = DateTime.Parse( "22.02.2019 8:00:45" ),
                Duration = TimeSpan.FromSeconds( duration ),
                Operation = operation,
            };

            // Action:
            builder.CheckDuration( action );

            // Assert:
            var res = builder.GetResult().productivityMap[ operation ].GetTime().TotalSeconds;
            Assert.That( res, Is.EqualTo( duration ) );
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

        [ Test, Category( "CheckPause" ) ]
        public void CheckPause__BothNotPacking_NextStartLessCurrentEnd__SetsCurrentEndEqualToNextStart ()
        {
            // curr: |_____________|..  - not paccking   ->   curr: |_______|.........  
            // next: ........|_______|. - not packing    ->   next: ........|_______|.

            // Arrange:
            var builder = GetBuilder();
            var gatheringOperation = new Operation { Name = "Current Operation", Group = OperationGroups.Gathering };

            var currentAction = new InventoryAction {
                StartTime = DateTime.Parse( "22.02.2019 8:00:00" ),
                Duration = TimeSpan.FromSeconds( 30 ),
                Operation = gatheringOperation,
            };

            var nextAction = new DoubleAddressAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:00:15" ),
                Duration = TimeSpan.FromSeconds( 30 ),
                Operation = gatheringOperation,
                DoubleAddressDetails = new List< DoubleAddressActionDetail >( new [] {
                    new DoubleAddressActionDetail { ProductQuantity = 1 }, 
                })
            };

            var next = builder.CheckDuration( nextAction );
            var current = builder.CheckDuration( currentAction );

            // Action:
            builder.CheckPause( current, next );

            // Assert:
             var res = builder.GetResult().productivityMap[ gatheringOperation ][currentAction];
            Assert.That( res.Duration.TotalSeconds, Is.EqualTo( 15 ) );
        }

        [ Test, Category( "CheckPause" ) ]
        public void CheckPause__BothNotPacking_NextStartAndEndLessCurrentEnd__SetsCurrentEndEqualToNextStart ()
        {
            // curr: |_____________|..  - not paccking   ->   curr: |_______|.........  
            // next: ........|____|...  - not packing    ->   next: ........|____|.....

            // Arrange:
            var builder = GetBuilder();
            var gatheringOperation = new Operation { Name = "Current Operation", Group = OperationGroups.Gathering };

            var currentAction = new InventoryAction {
                StartTime = DateTime.Parse( "22.02.2019 8:00:00" ),
                Duration = TimeSpan.FromSeconds( 60 ),
                Operation = gatheringOperation,
            };

            var nextAction = new DoubleAddressAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:00:15" ),
                Duration = TimeSpan.FromSeconds( 30 ),
                Operation = gatheringOperation,
                DoubleAddressDetails = new List< DoubleAddressActionDetail >( new [] {
                    new DoubleAddressActionDetail { ProductQuantity = 1 }, 
                })
            };

            var next = builder.CheckDuration( nextAction );
            var current = builder.CheckDuration( currentAction );

            // Action:
            builder.CheckPause( current, next );

            // Assert:
             var res = builder.GetResult().productivityMap[ gatheringOperation ][currentAction];
            Assert.That( res.Duration.TotalSeconds, Is.EqualTo( 15 ) );
        }


        [ Test, Category( "CheckPause" ) ]
        public void CheckPause__NextPacking_PackingStartLessCurrentEnd__SetsCurrentEndEqualToPackingStart ()
        {
            // curr: |_____________|..  - not paccking   ->   curr: |________|........  - not paccking
            // next: ........|_______|. - packing        ->   next: .........|______|. - packing

            // Arrange:
            var builder = GetBuilder();
            var gatheringOperation = new Operation { Name = "Current Operation", Group = OperationGroups.Gathering };
            var packingOperation = new Operation { Name = "Packing Operation", Group = OperationGroups.Packing };

            var currentAction = new InventoryAction {
                StartTime = DateTime.Parse( "22.02.2019 8:00:00" ),
                Duration = TimeSpan.FromSeconds( 30 ),
                Operation = gatheringOperation,
            };

            var packingAction = new DoubleAddressAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:00:15" ),
                Duration = TimeSpan.FromSeconds( 30 ),
                Operation = packingOperation,
                DoubleAddressDetails = new List< DoubleAddressActionDetail >( new [] {
                    new DoubleAddressActionDetail { ProductQuantity = 1 }, 
                })
            };

            var next = builder.CheckDuration( packingAction );
            var current = builder.CheckDuration( currentAction );

            // Action:
            builder.CheckPause( current, next );

            // Assert:
             var res = builder.GetResult().productivityMap[ gatheringOperation ][currentAction];
            Assert.That( res.Duration.TotalSeconds, Is.EqualTo( 15 ) );
        }

        [ Test, Category( "CheckPause" ) ]
        public void CheckPause__NextPacking_PackingStartAndEndLessCurrentEnd__SetsCurrentEndEqualToPackingStart ()
        {
            // curr: |_____________|..  - not paccking   ->   curr: |_______|.....  - not paccking
            // next: ........|___|..... - packing        ->   next: ........|___|. - packing

            // Arrange:
            var builder = GetBuilder();
            var gatheringOperation = new Operation { Name = "Current Operation", Group = OperationGroups.Gathering };
            var packingOperation = new Operation { Name = "Packing Operation", Group = OperationGroups.Packing };

            var currentAction = new InventoryAction {
                StartTime = DateTime.Parse( "22.02.2019 8:00:00" ),
                Duration = TimeSpan.FromSeconds( 60 ),
                Operation = gatheringOperation,
            };

            var packingAction = new DoubleAddressAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:00:15" ),
                Duration = TimeSpan.FromSeconds( 30 ),
                Operation = packingOperation,
                DoubleAddressDetails = new List< DoubleAddressActionDetail >( new [] {
                    new DoubleAddressActionDetail { ProductQuantity = 1 }, 
                })
            };

            var next = builder.CheckDuration( packingAction );
            var current = builder.CheckDuration( currentAction );

            // Action:
            builder.CheckPause( current, next );

            // Assert:
             var res = builder.GetResult().productivityMap[ gatheringOperation ][currentAction];
            Assert.That( res.Duration.TotalSeconds, Is.EqualTo( 15 ) );
        }

        [ Test, Category( "CheckPause" ) ]
        public void CheckPause__CurrentPacking_PackingStartAndEndLessCurrentEnd__SetsCurrentEndEqualToPackingStart ()
        {
            // curr: |_____________|..  - not paccking   ->   curr: |_______|.....  - not paccking
            // next: ........|___|..... - packing        ->   next: ........|___|. - packing

            // Arrange:
            var builder = GetBuilder();
            var gatheringOperation = new Operation { Name = "Current Operation", Group = OperationGroups.Gathering };
            var packingOperation = new Operation { Name = "Packing Operation", Group = OperationGroups.Packing };

            var currentAction = new InventoryAction {
                StartTime = DateTime.Parse( "22.02.2019 8:00:00" ),
                Duration = TimeSpan.FromSeconds( 60 ),
                Operation = gatheringOperation,
            };

            var packingAction = new DoubleAddressAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:00:15" ),
                Duration = TimeSpan.FromSeconds( 30 ),
                Operation = packingOperation,
                DoubleAddressDetails = new List< DoubleAddressActionDetail >( new [] {
                    new DoubleAddressActionDetail { ProductQuantity = 1 }, 
                })
            };

            var next = builder.CheckDuration( packingAction );
            var current = builder.CheckDuration( currentAction );

            // Action:
            builder.CheckPause( current, next );

            // Assert:
             var res = builder.GetResult().productivityMap[ gatheringOperation ][currentAction];
            Assert.That( res.Duration.TotalSeconds, Is.EqualTo( 15 ) );
        }


        [ Test, Category( "CheckPause" ) ]
        public void CheckPause__ThreePacking__SetsExpected ()
        {
            // in:                 d4                             d4            d4
            //          |_______________________|
            //                                        |_______________________|
            //                                                     |______________________|
            // time: ..12....13....14....15....16....17....18.....19....20....21....22....23....24
            //
            // out:                 d4                   d3                   d4
            //          |_______________________|__________________|______________________|
            // time: ..12....13....14....15....16....17....18.....19....20....21....22....23....24

            // Arrange:
            var builder = GetBuilder();
            var packingOperation = new Operation { Name = "Packing Operation", Group = OperationGroups.Packing };

            var action1 = new DoubleAddressAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:00:12" ),
                Duration = TimeSpan.FromSeconds( 4 ),
                Operation = packingOperation,
            };

            var action2 = new DoubleAddressAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:00:17" ),
                Duration = TimeSpan.FromSeconds( 4 ),
                Operation = packingOperation,
            };

            var action3 = new DoubleAddressAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:00:19" ),
                Duration = TimeSpan.FromSeconds( 4 ),
                Operation = packingOperation,
            };

            // Action:
            var next = builder.CheckDuration( action3 );
            var current = builder.CheckDuration( action2 );
            next = builder.CheckPause( current, next );
            current = builder.CheckDuration( action1 );
            next = builder.CheckPause( current, next );
            
            // Assert:
            var res = builder.GetResult();
            var total = res.productivityMap[ packingOperation ].GetTime().TotalSeconds;
            Assert.That( total, Is.EqualTo( 11 ) );
            Assert.That( res.downtimes, Is.Empty );
        }

        [ Test, Category( "CheckPause" ) ]
        public void CheckPause__ThreeNotPacking__SetsExpected ()
        {
            // in:                 d4                             d4            d4
            //          |_______________________|
            //                                        |_______________________|
            //                                                     |______________________|
            // time: ..12....13....14....15....16....17....18.....19....20....21....22....23....24
            //
            // out:                 d4                   d2                   d4
            //          |_______________________|    |____________|______________________|
            // time: ..12....13....14....15....16....17....18.....19....20....21....22....23....24

            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Packing Operation", Group = OperationGroups.Gathering };

            var action1 = new DoubleAddressAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:12:00" ),
                Duration = TimeSpan.FromMinutes( 4 ),
                Operation = operation,
            };

            var action2 = new DoubleAddressAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:17:00" ),
                Duration = TimeSpan.FromMinutes( 4 ),
                Operation = operation,
            };

            var action3 = new DoubleAddressAction() {
                StartTime = DateTime.Parse( "22.02.2019 8:19:00" ),
                Duration = TimeSpan.FromMinutes( 4 ),
                Operation = operation,
            };

            // Action:
            var next = builder.CheckDuration( action3 );
            var current = builder.CheckDuration( action2 );
            next = builder.CheckPause( current, next );
            current = builder.CheckDuration( action1 );
            next = builder.CheckPause( current, next );
            
            // Assert:
            var res = builder.GetResult();
            var total = (int)res.productivityMap[ operation ].GetTime().TotalMinutes;
            Assert.That( total, Is.EqualTo( 10 ) );
            Assert.That( res.downtimes, Is.Not.Empty );
            Assert.That( res.downtimes.Sum( d => d.Duration.TotalSeconds ), Is.EqualTo( 40 ) );
        }

        #endregion


        #region SubstractBreaks

        [ Category( "SubstractBreaks" ) ]
        [ TestCase( "22.02.2019 8:00:00", 54, "22.02.2019 8:59:00" ) ]
        [ TestCase( "22.02.2019 8:00:00", 55, "22.02.2019 8:59:00" ) ]
        [ TestCase( "22.02.2019 8:00:00", 55, "22.02.2019 9:00:00" ) ]
        [ TestCase( "22.02.2019 8:00:00", 56, "22.02.2019 9:00:00" ) ]
        [ TestCase( "22.02.2019 23:00:00", 55, "23.02.2019 0:00:00" ) ]
        [ TestCase( "22.02.2019 23:00:00", 56, "23.02.2019 0:00:00" ) ]
        [ TestCase( "22.02.2019 23:00:00", 55, "22.02.2019 23:59:00" ) ]
        [ TestCase( "22.02.2019 23:00:00", 55, "23.02.2019 0:00:20" ) ]
        public void SubstractBreaks_ActionBreakInBreakTime_DowntimesEmptyInResult ( string st0, int dur0, string st1 )
        {
            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Packing Operation", Group = OperationGroups.Gathering };

            var shortBreaks = new ShortBreakSchedule {
                Duration = TimeSpan.FromMinutes( 5 ),
                FirstBreakTime = new TimeSpan( 8, 55, 0),
                Periodicity = TimeSpan.FromHours( 1 )
            };

            var actions = new DoubleAddressAction[] {

                new DoubleAddressAction() {
                    StartTime = DateTime.Parse( st0 ),
                    Duration = TimeSpan.FromMinutes( dur0 ),
                    Operation = operation,
                },

                new DoubleAddressAction() {
                    StartTime = DateTime.Parse( st1 ),
                    Duration = TimeSpan.FromMinutes( 55 ),
                    Operation = operation,
                },
            };

            // Action:
            var next = builder.CheckDuration( actions[1] );
            var current = builder.CheckDuration( actions[0] );
            builder.CheckPause( current, next );
            builder.SubstractBreaks( shortBreaks );

            // Assert:
            var res = builder.GetResult();
            Assert.That( res.downtimes, Is.Empty );
        }


        [ Category( "SubstractBreaks" ) ]
        [ TestCase( "22.02.2019 8:00:00", 54, "22.02.2019 9:00:00" ) ]
        [ TestCase( "22.02.2019 23:00:00", 54, "23.02.2019 0:00:00" ) ]
        [ TestCase( "22.02.2019 8:00:00", 55, "22.02.2019 9:01:00" ) ]
        [ TestCase( "22.02.2019 23:00:00", 55, "23.02.2019 0:01:00" ) ]
        [ TestCase( "22.02.2019 23:00:00", 55, "23.02.2019 0:00:40" ) ]
        public void SubstractBreaks_ActionBreakGreaterBreakTime_DowntimesNotEmptyInResult ( string st0, int dur0, string st1 )
        {
            // Arrange:
            var builder = GetBuilder();
            var operation = new Operation { Name = "Packing Operation", Group = OperationGroups.Gathering };

            var shortBreaks = new ShortBreakSchedule {
                Duration = TimeSpan.FromMinutes( 5 ),
                FirstBreakTime = new TimeSpan( 8, 55, 0),
                Periodicity = TimeSpan.FromHours( 1 )
            };

            var actions = new DoubleAddressAction[] {

                new DoubleAddressAction() {
                    StartTime = DateTime.Parse( st0 ),
                    Duration = TimeSpan.FromMinutes( dur0 ),
                    Operation = operation,
                },

                new DoubleAddressAction() {
                    StartTime = DateTime.Parse( st1 ),
                    Duration = TimeSpan.FromMinutes( 55 ),
                    Operation = operation,
                },
            };

            // Action:
            var next = builder.CheckDuration( actions[1] );
            var current = builder.CheckDuration( actions[0] );
            builder.CheckPause( current, next );
            builder.SubstractBreaks( shortBreaks );

            // Assert:
            var res = builder.GetResult();
            Assert.That( res.downtimes, Is.Not.Empty );
        }


        [ Test, Category( "SubstractBreaks" ) ]
        public void SubstractBreaks__ShipmentActionWhenBreakTime_FirstDowntimeNotInTime__DowntimesNotEmptyInResult ()
        {
            // Arrange:
            var builder = GetBuilder();
            var shipmentOperation = new Operation { Name = "Packing Operation", Group = OperationGroups.Shipment };
            var gatheringOperation = new Operation { Name = "Packing Operation", Group = OperationGroups.Gathering };

            var shortBreaks = new ShortBreakSchedule {
                Duration = TimeSpan.FromMinutes( 5 ),
                FirstBreakTime = new TimeSpan( 8, 55, 0),
                Periodicity = TimeSpan.FromHours( 1 )
            };

            var actions = new EmployeeActionBase[] {

                new ShipmentAction() {
                    StartTime = DateTime.Parse( "22.02.2019 8:00:00" ),
                    Duration = TimeSpan.FromMinutes( 65 ),
                    Operation = shipmentOperation,
                },

                new DoubleAddressAction() {
                    StartTime = DateTime.Parse( "22.02.2019 8:10:20" ),
                    Duration = TimeSpan.FromMinutes( 55 ),
                    Operation = gatheringOperation,
                },
            };

            // Action:
            var next = builder.CheckDuration( actions[1] );
            var current = builder.CheckDuration( actions[0] );
            builder.CheckPause( current, next );
            builder.SubstractBreaks( shortBreaks );

            // Assert:
            var res = builder.GetResult();
            Assert.That( res.downtimes, Is.Empty );
        }


        [ Test, Category( "SubstractBreaks" ) ]
        public void SubstractBreaks__ShipmentActionWhenBreakTime_FirstDowntimeInTime__DowntimesNotEmptyInResult ()
        {
            // Arrange:
            var builder = GetBuilder();
            var shipmentOperation = new Operation { Name = "Packing Operation", Group = OperationGroups.Shipment };
            var gatheringOperation = new Operation { Name = "Packing Operation", Group = OperationGroups.Gathering };

            var shortBreaks = new ShortBreakSchedule {
                Duration = TimeSpan.FromMinutes( 5 ),
                FirstBreakTime = new TimeSpan( 8, 55, 0),
                Periodicity = TimeSpan.FromHours( 1 )
            };

            var actions = new EmployeeActionBase[] {

                new ShipmentAction() {
                    StartTime = DateTime.Parse( "22.02.2019 8:00:00" ),
                    Duration = TimeSpan.FromMinutes( 110 ),
                    Operation = shipmentOperation,
                },

                new DoubleAddressAction() {
                    StartTime = DateTime.Parse( "22.02.2019 10:10:20" ),
                    Duration = TimeSpan.FromMinutes( 55 ),
                    Operation = gatheringOperation,
                },
            };

            // Action:
            var next = builder.CheckDuration( actions[1] );
            var current = builder.CheckDuration( actions[0] );
            builder.CheckPause( current, next );
            builder.SubstractBreaks( shortBreaks );

            // Assert:
            var res = builder.GetResult();
            Assert.That( res.downtimes, Is.Not.Empty );
            Assert.That( res.downtimes.Count, Is.EqualTo( 2 ) );
        }


        #endregion


        #region Factory

        private ProductivityBuilder GetBuilder ( int thresholdInSec = 20 )
        {
            return new ProductivityBuilder();
        }

        #endregion
    }
}
