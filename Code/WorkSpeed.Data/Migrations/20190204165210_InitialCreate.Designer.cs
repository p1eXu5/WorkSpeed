﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkSpeed.Data.DataContexts;

namespace WorkSpeed.Data.Migrations
{
    [DbContext(typeof(WorkSpeedDbContext))]
    [Migration("20190204165210_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WorkSpeed.Data.Models.ActionDetails.DoubleAddressDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DoubleAddressActionId");

                    b.Property<byte?>("ReceiverCellAdressBox")
                        .IsRequired();

                    b.Property<string>("ReceiverCellAdressLetter")
                        .IsRequired();

                    b.Property<byte?>("ReceiverCellAdressRow")
                        .IsRequired();

                    b.Property<byte?>("ReceiverCellAdressSection")
                        .IsRequired();

                    b.Property<byte?>("ReceiverCellAdressShelf")
                        .IsRequired();

                    b.Property<byte?>("SenderCellAdressBox")
                        .IsRequired();

                    b.Property<string>("SenderCellAdressLetter")
                        .IsRequired();

                    b.Property<byte?>("SenderCellAdressRow")
                        .IsRequired();

                    b.Property<byte?>("SenderCellAdressSection")
                        .IsRequired();

                    b.Property<byte?>("SenderCellAdressShelf")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("DoubleAddressActionId");

                    b.HasIndex("ReceiverCellAdressLetter", "ReceiverCellAdressRow", "ReceiverCellAdressSection", "ReceiverCellAdressShelf", "ReceiverCellAdressBox");

                    b.HasIndex("SenderCellAdressLetter", "SenderCellAdressRow", "SenderCellAdressSection", "SenderCellAdressShelf", "SenderCellAdressBox");

                    b.ToTable("DoubleAddressDetails","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.ActionDetails.InventoryActionDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountingQuantity")
                        .HasColumnType("int");

                    b.Property<byte>("AddressBox");

                    b.Property<string>("AddressLetter")
                        .IsRequired();

                    b.Property<byte>("AddressRow");

                    b.Property<byte>("AddressSection");

                    b.Property<byte>("AddressShelf");

                    b.Property<int>("InventoryActionId");

                    b.HasKey("Id");

                    b.HasIndex("InventoryActionId");

                    b.HasIndex("AddressLetter", "AddressRow", "AddressSection", "AddressShelf", "AddressBox");

                    b.ToTable("InventoryDetails","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.ActionDetails.ReceptionActionDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("AddressBox");

                    b.Property<string>("AddressLetter")
                        .IsRequired();

                    b.Property<byte>("AddressRow");

                    b.Property<byte>("AddressSection");

                    b.Property<byte>("AddressShelf");

                    b.Property<bool>("IsClientScanning")
                        .HasColumnType("bit");

                    b.Property<int>("ReceptionActionId");

                    b.Property<short>("ScanQuantity")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("ReceptionActionId");

                    b.HasIndex("AddressLetter", "AddressRow", "AddressSection", "AddressShelf", "AddressBox");

                    b.ToTable("ReceptionDetails","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.ActionDetails.ShipmentActionDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float?>("ClientCargoQuantity")
                        .HasColumnType("real");

                    b.Property<float?>("CommonCargoQuantity")
                        .HasColumnType("real");

                    b.Property<int>("ShipmentActionId");

                    b.Property<float?>("Volume")
                        .HasColumnType("real");

                    b.Property<float?>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("ShipmentDetails","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Actions.DoubleAddressAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Document1CId")
                        .IsRequired();

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("EmployeeId")
                        .IsRequired();

                    b.Property<string>("EmployeeId1");

                    b.Property<int>("OperationId");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Document1CId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EmployeeId1");

                    b.HasIndex("OperationId");

                    b.ToTable("DoubleAddressActions","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Actions.InventoryAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Document1CId")
                        .IsRequired();

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("EmployeeId")
                        .IsRequired();

                    b.Property<string>("EmployeeId1");

                    b.Property<int>("OperationId");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Document1CId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EmployeeId1");

                    b.HasIndex("OperationId");

                    b.ToTable("InventoryActions","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Actions.OtherAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Document1CId")
                        .IsRequired();

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("EmployeeId")
                        .IsRequired();

                    b.Property<string>("EmployeeId1");

                    b.Property<int>("OperationId");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Document1CId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EmployeeId1");

                    b.HasIndex("OperationId");

                    b.ToTable("OtherActions","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Actions.ReceptionAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Document1CId")
                        .IsRequired();

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("EmployeeId")
                        .IsRequired();

                    b.Property<string>("EmployeeId1");

                    b.Property<int>("OperationId");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Document1CId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EmployeeId1");

                    b.HasIndex("OperationId");

                    b.ToTable("ReceptionActions","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Actions.ShipmentAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Document1CId")
                        .IsRequired();

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("EmployeeId")
                        .IsRequired();

                    b.Property<string>("EmployeeId1");

                    b.Property<int>("OperationId");

                    b.Property<int>("ShipmentDetailId");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Document1CId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EmployeeId1");

                    b.HasIndex("OperationId");

                    b.HasIndex("ShipmentDetailId")
                        .IsUnique();

                    b.ToTable("ShipmentActions","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Address", b =>
                {
                    b.Property<string>("Letter")
                        .HasColumnType("varchar(1)")
                        .HasMaxLength(1);

                    b.Property<byte>("Row");

                    b.Property<byte>("Section")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Shelf")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Box")
                        .HasColumnType("tinyint");

                    b.Property<string>("BoxType")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<float?>("Complexity")
                        .HasColumnType("real");

                    b.Property<float?>("Height")
                        .HasColumnType("real");

                    b.Property<float?>("Lenght")
                        .HasColumnType("real");

                    b.Property<float?>("MaxWeight")
                        .HasColumnType("real");

                    b.Property<int?>("PositionId");

                    b.Property<double?>("Volume")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("float")
                        .HasComputedColumnSql("[Width] * [Length] * [Height]");

                    b.Property<float?>("VolumeCoefficient")
                        .HasColumnType("real");

                    b.Property<float?>("Width")
                        .HasColumnType("real");

                    b.HasKey("Letter", "Row", "Section", "Shelf", "Box");

                    b.HasIndex("PositionId");

                    b.ToTable("Addresses","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasColumnType("varchar(16)");

                    b.Property<string>("InnerName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("OfficialName")
                        .HasColumnType("varchar(255)");

                    b.Property<decimal?>("SalaryPerOneHour")
                        .HasColumnType("decimal");

                    b.HasKey("Id");

                    b.ToTable("Appointments","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Category", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasColumnType("varchar(16)");

                    b.Property<double>("MaxVolume")
                        .HasColumnType("float");

                    b.Property<double>("MinVolume")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Categories","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.CategorySet", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Category0Id");

                    b.Property<int?>("Category1Id");

                    b.Property<int?>("Category2Id");

                    b.Property<int?>("Category3Id");

                    b.Property<int?>("Category4Id");

                    b.Property<int?>("Category5Id");

                    b.Property<int?>("Category6Id");

                    b.Property<int?>("Category7Id");

                    b.Property<int?>("Category8Id");

                    b.Property<int?>("Category9Id");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Category0Id");

                    b.HasIndex("Category1Id");

                    b.HasIndex("Category2Id");

                    b.HasIndex("Category3Id");

                    b.HasIndex("Category4Id");

                    b.HasIndex("Category5Id");

                    b.HasIndex("Category6Id");

                    b.HasIndex("Category7Id");

                    b.HasIndex("Category8Id");

                    b.HasIndex("Category9Id");

                    b.ToTable("CategorySets","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Document1C", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Documents","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Employee", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(7)")
                        .HasMaxLength(7);

                    b.Property<int?>("AppointmentId");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSmoker")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("PositionId");

                    b.Property<DateTime?>("ProbationEnd")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<int?>("RankNumber");

                    b.Property<int?>("ShiftId");

                    b.Property<int?>("ShortBreakScheduleId");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId");

                    b.HasIndex("PositionId");

                    b.HasIndex("RankNumber");

                    b.HasIndex("ShiftId");

                    b.HasIndex("ShortBreakScheduleId");

                    b.ToTable("Employees","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Operation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float?>("Complexity")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("OperationGroup")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Operations","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<float?>("Complexity")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Positions","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float?>("CartonHeight")
                        .HasColumnType("real");

                    b.Property<float?>("CartonLength")
                        .HasColumnType("real");

                    b.Property<int?>("CartonQuantity")
                        .HasColumnType("int");

                    b.Property<float?>("CartonVolume")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("real")
                        .HasComputedColumnSql("[ItemWidth] * [ItemLength] * [ItemHeight] * [CartonQuantity]");

                    b.Property<float?>("CartonWeight")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("real")
                        .HasComputedColumnSql("[ItemWeight] * [CartonQuantity]");

                    b.Property<float?>("CartonWidth")
                        .HasColumnType("real");

                    b.Property<float?>("GatheringComplexity")
                        .HasColumnType("real");

                    b.Property<float?>("InventoryComplexity")
                        .HasColumnType("real");

                    b.Property<bool>("IsGroup")
                        .HasColumnType("bit");

                    b.Property<float?>("ItemHeight")
                        .HasColumnType("real");

                    b.Property<float?>("ItemLength")
                        .HasColumnType("real");

                    b.Property<float?>("ItemVolume")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("real")
                        .HasComputedColumnSql("[ItemWidth] * [ItemLength] * [ItemHeight]");

                    b.Property<float?>("ItemWeight")
                        .HasColumnType("real");

                    b.Property<float?>("ItemWidth")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(196);

                    b.Property<float?>("PackagingComplexity")
                        .HasColumnType("real");

                    b.Property<int?>("ParentId");

                    b.Property<float?>("PlacingComplexity")
                        .HasColumnType("real");

                    b.Property<float?>("ScanningComplexity")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Products","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Rank", b =>
                {
                    b.Property<int>("Number");

                    b.Property<decimal?>("OneHourCost")
                        .HasColumnType("decimal");

                    b.HasKey("Number");

                    b.ToTable("Ranks","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Shift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<TimeSpan>("LunchDuration")
                        .HasColumnType("time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<TimeSpan?>("RestTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("ShiftDuration")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.ToTable("Shifts","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.ShortBreakSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("FirstBreakTime")
                        .HasColumnType("time");

                    b.Property<bool>("IsForSmokers")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(25)");

                    b.Property<TimeSpan>("Periodicity")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.ToTable("ShortBreaks","dbo");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.ActionDetails.DoubleAddressDetail", b =>
                {
                    b.HasOne("WorkSpeed.Data.Models.Actions.DoubleAddressAction", "DoubleAddressAction")
                        .WithMany("DoubleAddressDetails")
                        .HasForeignKey("DoubleAddressActionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Address", "ReceiverCellAdress")
                        .WithMany()
                        .HasForeignKey("ReceiverCellAdressLetter", "ReceiverCellAdressRow", "ReceiverCellAdressSection", "ReceiverCellAdressShelf", "ReceiverCellAdressBox")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Address", "SenderCellAdress")
                        .WithMany()
                        .HasForeignKey("SenderCellAdressLetter", "SenderCellAdressRow", "SenderCellAdressSection", "SenderCellAdressShelf", "SenderCellAdressBox")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.ActionDetails.InventoryActionDetail", b =>
                {
                    b.HasOne("WorkSpeed.Data.Models.Actions.InventoryAction", "InventoryAction")
                        .WithMany("InventoryActionDetails")
                        .HasForeignKey("InventoryActionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressLetter", "AddressRow", "AddressSection", "AddressShelf", "AddressBox")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.ActionDetails.ReceptionActionDetail", b =>
                {
                    b.HasOne("WorkSpeed.Data.Models.Actions.ReceptionAction", "ReceptionAction")
                        .WithMany("ReceptionActionDetails")
                        .HasForeignKey("ReceptionActionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressLetter", "AddressRow", "AddressSection", "AddressShelf", "AddressBox")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Actions.DoubleAddressAction", b =>
                {
                    b.HasOne("WorkSpeed.Data.Models.Document1C", "Document1C")
                        .WithMany()
                        .HasForeignKey("Document1CId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Employee")
                        .WithMany("DoubleAddressActions")
                        .HasForeignKey("EmployeeId1");

                    b.HasOne("WorkSpeed.Data.Models.Operation", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Actions.InventoryAction", b =>
                {
                    b.HasOne("WorkSpeed.Data.Models.Document1C", "Document1C")
                        .WithMany()
                        .HasForeignKey("Document1CId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Employee")
                        .WithMany("InventoryActions")
                        .HasForeignKey("EmployeeId1");

                    b.HasOne("WorkSpeed.Data.Models.Operation", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Actions.OtherAction", b =>
                {
                    b.HasOne("WorkSpeed.Data.Models.Document1C", "Document1C")
                        .WithMany()
                        .HasForeignKey("Document1CId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Employee")
                        .WithMany("OtherActions")
                        .HasForeignKey("EmployeeId1");

                    b.HasOne("WorkSpeed.Data.Models.Operation", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Actions.ReceptionAction", b =>
                {
                    b.HasOne("WorkSpeed.Data.Models.Document1C", "Document1C")
                        .WithMany()
                        .HasForeignKey("Document1CId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Employee")
                        .WithMany("ReceptionActions")
                        .HasForeignKey("EmployeeId1");

                    b.HasOne("WorkSpeed.Data.Models.Operation", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Actions.ShipmentAction", b =>
                {
                    b.HasOne("WorkSpeed.Data.Models.Document1C", "Document1C")
                        .WithMany()
                        .HasForeignKey("Document1CId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.Employee")
                        .WithMany("ShipmentActions")
                        .HasForeignKey("EmployeeId1");

                    b.HasOne("WorkSpeed.Data.Models.Operation", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkSpeed.Data.Models.ActionDetails.ShipmentActionDetail", "ShipmentActionDetail")
                        .WithOne("ShipmentAction")
                        .HasForeignKey("WorkSpeed.Data.Models.Actions.ShipmentAction", "ShipmentDetailId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Address", b =>
                {
                    b.HasOne("WorkSpeed.Data.Models.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.CategorySet", b =>
                {
                    b.HasOne("WorkSpeed.Data.Models.Category", "Category0")
                        .WithMany()
                        .HasForeignKey("Category0Id");

                    b.HasOne("WorkSpeed.Data.Models.Category", "Category1")
                        .WithMany()
                        .HasForeignKey("Category1Id");

                    b.HasOne("WorkSpeed.Data.Models.Category", "Category2")
                        .WithMany()
                        .HasForeignKey("Category2Id");

                    b.HasOne("WorkSpeed.Data.Models.Category", "Category3")
                        .WithMany()
                        .HasForeignKey("Category3Id");

                    b.HasOne("WorkSpeed.Data.Models.Category", "Category4")
                        .WithMany()
                        .HasForeignKey("Category4Id");

                    b.HasOne("WorkSpeed.Data.Models.Category", "Category5")
                        .WithMany()
                        .HasForeignKey("Category5Id");

                    b.HasOne("WorkSpeed.Data.Models.Category", "Category6")
                        .WithMany()
                        .HasForeignKey("Category6Id");

                    b.HasOne("WorkSpeed.Data.Models.Category", "Category7")
                        .WithMany()
                        .HasForeignKey("Category7Id");

                    b.HasOne("WorkSpeed.Data.Models.Category", "Category8")
                        .WithMany()
                        .HasForeignKey("Category8Id");

                    b.HasOne("WorkSpeed.Data.Models.Category", "Category9")
                        .WithMany()
                        .HasForeignKey("Category9Id");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Employee", b =>
                {
                    b.HasOne("WorkSpeed.Data.Models.Appointment", "Appointment")
                        .WithMany()
                        .HasForeignKey("AppointmentId");

                    b.HasOne("WorkSpeed.Data.Models.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId");

                    b.HasOne("WorkSpeed.Data.Models.Rank", "Rank")
                        .WithMany()
                        .HasForeignKey("RankNumber");

                    b.HasOne("WorkSpeed.Data.Models.Shift", "Shift")
                        .WithMany()
                        .HasForeignKey("ShiftId");

                    b.HasOne("WorkSpeed.Data.Models.ShortBreakSchedule", "ShortBreakSchedule")
                        .WithMany()
                        .HasForeignKey("ShortBreakScheduleId");
                });

            modelBuilder.Entity("WorkSpeed.Data.Models.Product", b =>
                {
                    b.HasOne("WorkSpeed.Data.Models.Product", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .HasConstraintName("ForeignKey_ProductChild_ProductParent");
                });
#pragma warning restore 612, 618
        }
    }
}
