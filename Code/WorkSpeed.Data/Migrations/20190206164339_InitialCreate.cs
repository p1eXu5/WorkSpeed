using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkSpeed.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Appointments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OfficialName = table.Column<string>(type: "varchar(255)", nullable: true),
                    InnerName = table.Column<string>(type: "varchar(255)", nullable: false),
                    SalaryPerOneHour = table.Column<decimal>(type: "decimal", nullable: true),
                    Abbreviations = table.Column<string>(type: "varchar(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Abbreviation = table.Column<string>(type: "varchar(16)", nullable: false),
                    MinVolume = table.Column<double>(type: "float", nullable: false),
                    MaxVolume = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    OperationGroup = table.Column<string>(type: "varchar(50)", nullable: false),
                    Complexity = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Complexity = table.Column<float>(type: "real", nullable: true),
                    Abbreviation = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 196, nullable: false),
                    IsGroup = table.Column<bool>(type: "bit", nullable: false),
                    ItemLength = table.Column<float>(type: "real", nullable: true),
                    ItemWidth = table.Column<float>(type: "real", nullable: true),
                    ItemHeight = table.Column<float>(type: "real", nullable: true),
                    ItemWeight = table.Column<float>(type: "real", nullable: true),
                    ItemVolume = table.Column<float>(type: "real", nullable: true, computedColumnSql: "[ItemWidth] * [ItemLength] * [ItemHeight]"),
                    CartonLength = table.Column<float>(type: "real", nullable: true),
                    CartonWidth = table.Column<float>(type: "real", nullable: true),
                    CartonHeight = table.Column<float>(type: "real", nullable: true),
                    CartonQuantity = table.Column<int>(type: "int", nullable: true),
                    CartonWeight = table.Column<float>(type: "real", nullable: true, computedColumnSql: "[ItemWeight] * [CartonQuantity]"),
                    CartonVolume = table.Column<float>(type: "real", nullable: true, computedColumnSql: "[ItemWidth] * [ItemLength] * [ItemHeight] * [CartonQuantity]"),
                    GatheringComplexity = table.Column<float>(type: "real", nullable: true),
                    PackagingComplexity = table.Column<float>(type: "real", nullable: true),
                    ScanningComplexity = table.Column<float>(type: "real", nullable: true),
                    InventoryComplexity = table.Column<float>(type: "real", nullable: true),
                    PlacingComplexity = table.Column<float>(type: "real", nullable: true),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "ForeignKey_ProductChild_ProductParent",
                        column: x => x.ParentId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ranks",
                schema: "dbo",
                columns: table => new
                {
                    Number = table.Column<int>(nullable: false),
                    OneHourCost = table.Column<decimal>(type: "decimal", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.Number);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ShiftDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    LunchDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    RestTime = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Weight = table.Column<float>(type: "real", nullable: true),
                    Volume = table.Column<float>(type: "real", nullable: true),
                    ClientCargoQuantity = table.Column<float>(type: "real", nullable: true),
                    CommonCargoQuantity = table.Column<float>(type: "real", nullable: true),
                    ShipmentActionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShortBreaks",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Periodicity = table.Column<TimeSpan>(type: "time", nullable: false),
                    FirstBreakTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsForSmokers = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortBreaks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategorySets",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", nullable: true),
                    Category0Id = table.Column<int>(nullable: true),
                    Category1Id = table.Column<int>(nullable: true),
                    Category2Id = table.Column<int>(nullable: true),
                    Category3Id = table.Column<int>(nullable: true),
                    Category4Id = table.Column<int>(nullable: true),
                    Category5Id = table.Column<int>(nullable: true),
                    Category6Id = table.Column<int>(nullable: true),
                    Category7Id = table.Column<int>(nullable: true),
                    Category8Id = table.Column<int>(nullable: true),
                    Category9Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategorySets_Categories_Category0Id",
                        column: x => x.Category0Id,
                        principalSchema: "dbo",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategorySets_Categories_Category1Id",
                        column: x => x.Category1Id,
                        principalSchema: "dbo",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategorySets_Categories_Category2Id",
                        column: x => x.Category2Id,
                        principalSchema: "dbo",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategorySets_Categories_Category3Id",
                        column: x => x.Category3Id,
                        principalSchema: "dbo",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategorySets_Categories_Category4Id",
                        column: x => x.Category4Id,
                        principalSchema: "dbo",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategorySets_Categories_Category5Id",
                        column: x => x.Category5Id,
                        principalSchema: "dbo",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategorySets_Categories_Category6Id",
                        column: x => x.Category6Id,
                        principalSchema: "dbo",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategorySets_Categories_Category7Id",
                        column: x => x.Category7Id,
                        principalSchema: "dbo",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategorySets_Categories_Category8Id",
                        column: x => x.Category8Id,
                        principalSchema: "dbo",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategorySets_Categories_Category9Id",
                        column: x => x.Category9Id,
                        principalSchema: "dbo",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "dbo",
                columns: table => new
                {
                    Letter = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false),
                    Section = table.Column<byte>(type: "tinyint", nullable: false),
                    Row = table.Column<byte>(nullable: false),
                    Shelf = table.Column<byte>(type: "tinyint", nullable: false),
                    Box = table.Column<byte>(type: "tinyint", nullable: false),
                    BoxType = table.Column<string>(type: "varchar(50)", nullable: false),
                    Length = table.Column<float>(type: "real", nullable: true),
                    Width = table.Column<float>(type: "real", nullable: true),
                    Height = table.Column<float>(type: "real", nullable: true),
                    Volume = table.Column<double>(type: "float", nullable: true, computedColumnSql: "[Width] * [Length] * [Height]"),
                    MaxWeight = table.Column<float>(type: "real", nullable: true),
                    VolumeCoefficient = table.Column<float>(type: "real", nullable: true),
                    Complexity = table.Column<float>(type: "real", nullable: true),
                    PositionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => new { x.Letter, x.Row, x.Section, x.Shelf, x.Box });
                    table.ForeignKey(
                        name: "FK_Addresses_Positions_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "dbo",
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSmoker = table.Column<bool>(type: "bit", nullable: false),
                    ProbationEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PositionId = table.Column<int>(nullable: true),
                    RankNumber = table.Column<int>(nullable: true),
                    AppointmentId = table.Column<int>(nullable: true),
                    ShiftId = table.Column<int>(nullable: true),
                    ShortBreakScheduleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalSchema: "dbo",
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Positions_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "dbo",
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Ranks_RankNumber",
                        column: x => x.RankNumber,
                        principalSchema: "dbo",
                        principalTable: "Ranks",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: "dbo",
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_ShortBreaks_ShortBreakScheduleId",
                        column: x => x.ShortBreakScheduleId,
                        principalSchema: "dbo",
                        principalTable: "ShortBreaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoubleAddressActions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    EmployeeId = table.Column<string>(nullable: false),
                    Document1CId = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    EmployeeId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoubleAddressActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoubleAddressActions_Documents_Document1CId",
                        column: x => x.Document1CId,
                        principalSchema: "dbo",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoubleAddressActions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoubleAddressActions_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoubleAddressActions_Operations_OperationId",
                        column: x => x.OperationId,
                        principalSchema: "dbo",
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryActions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    EmployeeId = table.Column<string>(nullable: false),
                    Document1CId = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    EmployeeId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryActions_Documents_Document1CId",
                        column: x => x.Document1CId,
                        principalSchema: "dbo",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryActions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryActions_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryActions_Operations_OperationId",
                        column: x => x.OperationId,
                        principalSchema: "dbo",
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtherActions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    EmployeeId = table.Column<string>(nullable: false),
                    Document1CId = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    EmployeeId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherActions_Documents_Document1CId",
                        column: x => x.Document1CId,
                        principalSchema: "dbo",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OtherActions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OtherActions_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OtherActions_Operations_OperationId",
                        column: x => x.OperationId,
                        principalSchema: "dbo",
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceptionActions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    EmployeeId = table.Column<string>(nullable: false),
                    Document1CId = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    EmployeeId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceptionActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceptionActions_Documents_Document1CId",
                        column: x => x.Document1CId,
                        principalSchema: "dbo",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceptionActions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceptionActions_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceptionActions_Operations_OperationId",
                        column: x => x.OperationId,
                        principalSchema: "dbo",
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentActions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    EmployeeId = table.Column<string>(nullable: false),
                    Document1CId = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    ShipmentDetailId = table.Column<int>(nullable: false),
                    EmployeeId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipmentActions_Documents_Document1CId",
                        column: x => x.Document1CId,
                        principalSchema: "dbo",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShipmentActions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShipmentActions_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShipmentActions_Operations_OperationId",
                        column: x => x.OperationId,
                        principalSchema: "dbo",
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShipmentActions_ShipmentDetails_ShipmentDetailId",
                        column: x => x.ShipmentDetailId,
                        principalSchema: "dbo",
                        principalTable: "ShipmentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoubleAddressDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SenderCellAdressLetter = table.Column<string>(nullable: false),
                    SenderCellAdressRow = table.Column<byte>(nullable: false),
                    SenderCellAdressSection = table.Column<byte>(nullable: false),
                    SenderCellAdressShelf = table.Column<byte>(nullable: false),
                    SenderCellAdressBox = table.Column<byte>(nullable: false),
                    ReceiverCellAdressLetter = table.Column<string>(nullable: false),
                    ReceiverCellAdressRow = table.Column<byte>(nullable: false),
                    ReceiverCellAdressSection = table.Column<byte>(nullable: false),
                    ReceiverCellAdressShelf = table.Column<byte>(nullable: false),
                    ReceiverCellAdressBox = table.Column<byte>(nullable: false),
                    DoubleAddressActionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoubleAddressDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoubleAddressDetails_DoubleAddressActions_DoubleAddressActionId",
                        column: x => x.DoubleAddressActionId,
                        principalSchema: "dbo",
                        principalTable: "DoubleAddressActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoubleAddressDetails_Addresses_ReceiverCellAdressLetter_ReceiverCellAdressRow_ReceiverCellAdressSection_ReceiverCellAdressSh~",
                        columns: x => new { x.ReceiverCellAdressLetter, x.ReceiverCellAdressRow, x.ReceiverCellAdressSection, x.ReceiverCellAdressShelf, x.ReceiverCellAdressBox },
                        principalSchema: "dbo",
                        principalTable: "Addresses",
                        principalColumns: new[] { "Letter", "Row", "Section", "Shelf", "Box" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoubleAddressDetails_Addresses_SenderCellAdressLetter_SenderCellAdressRow_SenderCellAdressSection_SenderCellAdressShelf_Send~",
                        columns: x => new { x.SenderCellAdressLetter, x.SenderCellAdressRow, x.SenderCellAdressSection, x.SenderCellAdressShelf, x.SenderCellAdressBox },
                        principalSchema: "dbo",
                        principalTable: "Addresses",
                        principalColumns: new[] { "Letter", "Row", "Section", "Shelf", "Box" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressLetter = table.Column<string>(nullable: false),
                    AddressRow = table.Column<byte>(nullable: false),
                    AddressSection = table.Column<byte>(nullable: false),
                    AddressShelf = table.Column<byte>(nullable: false),
                    AddressBox = table.Column<byte>(nullable: false),
                    AccountingQuantity = table.Column<int>(type: "int", nullable: false),
                    InventoryActionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryDetails_InventoryActions_InventoryActionId",
                        column: x => x.InventoryActionId,
                        principalSchema: "dbo",
                        principalTable: "InventoryActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryDetails_Addresses_AddressLetter_AddressRow_AddressSection_AddressShelf_AddressBox",
                        columns: x => new { x.AddressLetter, x.AddressRow, x.AddressSection, x.AddressShelf, x.AddressBox },
                        principalSchema: "dbo",
                        principalTable: "Addresses",
                        principalColumns: new[] { "Letter", "Row", "Section", "Shelf", "Box" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceptionDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressLetter = table.Column<string>(nullable: false),
                    AddressRow = table.Column<byte>(nullable: false),
                    AddressSection = table.Column<byte>(nullable: false),
                    AddressShelf = table.Column<byte>(nullable: false),
                    AddressBox = table.Column<byte>(nullable: false),
                    ScanQuantity = table.Column<short>(type: "smallint", nullable: false),
                    IsClientScanning = table.Column<bool>(type: "bit", nullable: false),
                    ReceptionActionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceptionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceptionDetails_ReceptionActions_ReceptionActionId",
                        column: x => x.ReceptionActionId,
                        principalSchema: "dbo",
                        principalTable: "ReceptionActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceptionDetails_Addresses_AddressLetter_AddressRow_AddressSection_AddressShelf_AddressBox",
                        columns: x => new { x.AddressLetter, x.AddressRow, x.AddressSection, x.AddressShelf, x.AddressBox },
                        principalSchema: "dbo",
                        principalTable: "Addresses",
                        principalColumns: new[] { "Letter", "Row", "Section", "Shelf", "Box" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PositionId",
                schema: "dbo",
                table: "Addresses",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySets_Category0Id",
                schema: "dbo",
                table: "CategorySets",
                column: "Category0Id");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySets_Category1Id",
                schema: "dbo",
                table: "CategorySets",
                column: "Category1Id");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySets_Category2Id",
                schema: "dbo",
                table: "CategorySets",
                column: "Category2Id");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySets_Category3Id",
                schema: "dbo",
                table: "CategorySets",
                column: "Category3Id");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySets_Category4Id",
                schema: "dbo",
                table: "CategorySets",
                column: "Category4Id");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySets_Category5Id",
                schema: "dbo",
                table: "CategorySets",
                column: "Category5Id");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySets_Category6Id",
                schema: "dbo",
                table: "CategorySets",
                column: "Category6Id");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySets_Category7Id",
                schema: "dbo",
                table: "CategorySets",
                column: "Category7Id");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySets_Category8Id",
                schema: "dbo",
                table: "CategorySets",
                column: "Category8Id");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySets_Category9Id",
                schema: "dbo",
                table: "CategorySets",
                column: "Category9Id");

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressActions_Document1CId",
                schema: "dbo",
                table: "DoubleAddressActions",
                column: "Document1CId");

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressActions_EmployeeId",
                schema: "dbo",
                table: "DoubleAddressActions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressActions_EmployeeId1",
                schema: "dbo",
                table: "DoubleAddressActions",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressActions_OperationId",
                schema: "dbo",
                table: "DoubleAddressActions",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressDetails_DoubleAddressActionId",
                schema: "dbo",
                table: "DoubleAddressDetails",
                column: "DoubleAddressActionId");

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressDetails_ReceiverCellAdressLetter_ReceiverCellAdressRow_ReceiverCellAdressSection_ReceiverCellAdressShelf_Receiv~",
                schema: "dbo",
                table: "DoubleAddressDetails",
                columns: new[] { "ReceiverCellAdressLetter", "ReceiverCellAdressRow", "ReceiverCellAdressSection", "ReceiverCellAdressShelf", "ReceiverCellAdressBox" });

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressDetails_SenderCellAdressLetter_SenderCellAdressRow_SenderCellAdressSection_SenderCellAdressShelf_SenderCellAdre~",
                schema: "dbo",
                table: "DoubleAddressDetails",
                columns: new[] { "SenderCellAdressLetter", "SenderCellAdressRow", "SenderCellAdressSection", "SenderCellAdressShelf", "SenderCellAdressBox" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AppointmentId",
                schema: "dbo",
                table: "Employees",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionId",
                schema: "dbo",
                table: "Employees",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RankNumber",
                schema: "dbo",
                table: "Employees",
                column: "RankNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ShiftId",
                schema: "dbo",
                table: "Employees",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ShortBreakScheduleId",
                schema: "dbo",
                table: "Employees",
                column: "ShortBreakScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryActions_Document1CId",
                schema: "dbo",
                table: "InventoryActions",
                column: "Document1CId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryActions_EmployeeId",
                schema: "dbo",
                table: "InventoryActions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryActions_EmployeeId1",
                schema: "dbo",
                table: "InventoryActions",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryActions_OperationId",
                schema: "dbo",
                table: "InventoryActions",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_InventoryActionId",
                schema: "dbo",
                table: "InventoryDetails",
                column: "InventoryActionId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_AddressLetter_AddressRow_AddressSection_AddressShelf_AddressBox",
                schema: "dbo",
                table: "InventoryDetails",
                columns: new[] { "AddressLetter", "AddressRow", "AddressSection", "AddressShelf", "AddressBox" });

            migrationBuilder.CreateIndex(
                name: "IX_OtherActions_Document1CId",
                schema: "dbo",
                table: "OtherActions",
                column: "Document1CId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherActions_EmployeeId",
                schema: "dbo",
                table: "OtherActions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherActions_EmployeeId1",
                schema: "dbo",
                table: "OtherActions",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_OtherActions_OperationId",
                schema: "dbo",
                table: "OtherActions",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ParentId",
                schema: "dbo",
                table: "Products",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptionActions_Document1CId",
                schema: "dbo",
                table: "ReceptionActions",
                column: "Document1CId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptionActions_EmployeeId",
                schema: "dbo",
                table: "ReceptionActions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptionActions_EmployeeId1",
                schema: "dbo",
                table: "ReceptionActions",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptionActions_OperationId",
                schema: "dbo",
                table: "ReceptionActions",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptionDetails_ReceptionActionId",
                schema: "dbo",
                table: "ReceptionDetails",
                column: "ReceptionActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptionDetails_AddressLetter_AddressRow_AddressSection_AddressShelf_AddressBox",
                schema: "dbo",
                table: "ReceptionDetails",
                columns: new[] { "AddressLetter", "AddressRow", "AddressSection", "AddressShelf", "AddressBox" });

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentActions_Document1CId",
                schema: "dbo",
                table: "ShipmentActions",
                column: "Document1CId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentActions_EmployeeId",
                schema: "dbo",
                table: "ShipmentActions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentActions_EmployeeId1",
                schema: "dbo",
                table: "ShipmentActions",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentActions_OperationId",
                schema: "dbo",
                table: "ShipmentActions",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentActions_ShipmentDetailId",
                schema: "dbo",
                table: "ShipmentActions",
                column: "ShipmentDetailId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategorySets",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DoubleAddressDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OtherActions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ReceptionDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ShipmentActions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DoubleAddressActions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryActions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ReceptionActions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ShipmentDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Operations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Appointments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Positions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Ranks",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Shifts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ShortBreaks",
                schema: "dbo");
        }
    }
}
