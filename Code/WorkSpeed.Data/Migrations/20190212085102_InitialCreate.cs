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
                    OfficialName = table.Column<string>(type: "varchar(50)", nullable: true),
                    InnerName = table.Column<string>(type: "varchar(50)", nullable: false),
                    SalaryPerOneHour = table.Column<decimal>(type: "decimal", nullable: true),
                    Abbreviations = table.Column<string>(type: "varchar(50)", nullable: false)
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
                    MinVolume = table.Column<double>(type: "float", nullable: true),
                    MaxVolume = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategorySets",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    OperationGroup = table.Column<string>(type: "varchar(10)", nullable: false),
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
                    Abbreviations = table.Column<string>(type: "varchar(50)", nullable: false)
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
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Lunch = table.Column<TimeSpan>(type: "time", nullable: false),
                    RestTime = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
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
                    FirstBreakTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortBreaks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryCategorySet",
                columns: table => new
                {
                    CategorySetId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCategorySet", x => new { x.CategoryId, x.CategorySetId });
                    table.ForeignKey(
                        name: "FK_CategoryCategorySet_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "dbo",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryCategorySet_CategorySets_CategorySetId",
                        column: x => x.CategorySetId,
                        principalSchema: "dbo",
                        principalTable: "CategorySets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    IsSmoker = table.Column<bool>(type: "bit", nullable: true),
                    ProbationEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    Id = table.Column<string>(type: "varchar(11)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    DocumentName = table.Column<string>(type: "varchar(100)", nullable: true),
                    EmployeeId = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    EmployeeId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoubleAddressActions", x => x.Id);
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
                    Id = table.Column<string>(type: "varchar(11)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    DocumentName = table.Column<string>(type: "varchar(100)", nullable: true),
                    EmployeeId = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    EmployeeId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryActions", x => x.Id);
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
                    Id = table.Column<string>(type: "varchar(11)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    DocumentName = table.Column<string>(type: "varchar(100)", nullable: true),
                    EmployeeId = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    EmployeeId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherActions", x => x.Id);
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
                    Id = table.Column<string>(type: "varchar(11)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    DocumentName = table.Column<string>(type: "varchar(100)", nullable: true),
                    EmployeeId = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    EmployeeId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceptionActions", x => x.Id);
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
                    Id = table.Column<string>(type: "varchar(11)", nullable: false),
                    EmployeeId = table.Column<string>(type: "varchar(7)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    DocumentName = table.Column<string>(type: "varchar(100)", nullable: true),
                    OperationId = table.Column<int>(nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: true),
                    Volume = table.Column<float>(type: "real", nullable: true),
                    ClientCargoQuantity = table.Column<float>(type: "real", nullable: true),
                    CommonCargoQuantity = table.Column<float>(type: "real", nullable: true),
                    EmployeeId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentActions", x => new { x.Id, x.EmployeeId });
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
                });

            migrationBuilder.CreateTable(
                name: "DoubleAddressDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductQuantity = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    SenderAddressLetter = table.Column<string>(nullable: true),
                    SenderAddressRow = table.Column<byte>(nullable: true),
                    SenderAddressSection = table.Column<byte>(nullable: true),
                    SenderAddressShelf = table.Column<byte>(nullable: true),
                    SenderAddressBox = table.Column<byte>(nullable: true),
                    ReceiverAddressLetter = table.Column<string>(nullable: true),
                    ReceiverAddressRow = table.Column<byte>(nullable: true),
                    ReceiverAddressSection = table.Column<byte>(nullable: true),
                    ReceiverAddressShelf = table.Column<byte>(nullable: true),
                    ReceiverAddressBox = table.Column<byte>(nullable: true),
                    DoubleAddressActionId = table.Column<string>(nullable: false)
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
                        name: "FK_DoubleAddressDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoubleAddressDetails_Addresses_ReceiverAddressLetter_ReceiverAddressRow_ReceiverAddressSection_ReceiverAddressShelf_Receiver~",
                        columns: x => new { x.ReceiverAddressLetter, x.ReceiverAddressRow, x.ReceiverAddressSection, x.ReceiverAddressShelf, x.ReceiverAddressBox },
                        principalSchema: "dbo",
                        principalTable: "Addresses",
                        principalColumns: new[] { "Letter", "Row", "Section", "Shelf", "Box" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoubleAddressDetails_Addresses_SenderAddressLetter_SenderAddressRow_SenderAddressSection_SenderAddressShelf_SenderAddressBox",
                        columns: x => new { x.SenderAddressLetter, x.SenderAddressRow, x.SenderAddressSection, x.SenderAddressShelf, x.SenderAddressBox },
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
                    ProductQuantity = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    AddressLetter = table.Column<string>(nullable: false),
                    AddressRow = table.Column<byte>(nullable: false),
                    AddressSection = table.Column<byte>(nullable: false),
                    AddressShelf = table.Column<byte>(nullable: false),
                    AddressBox = table.Column<byte>(nullable: false),
                    AccountingQuantity = table.Column<int>(type: "int", nullable: false),
                    InventoryActionId = table.Column<string>(nullable: false)
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
                        name: "FK_InventoryDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
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
                    ProductQuantity = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    AddressLetter = table.Column<string>(nullable: false),
                    AddressRow = table.Column<byte>(nullable: false),
                    AddressSection = table.Column<byte>(nullable: false),
                    AddressShelf = table.Column<byte>(nullable: false),
                    AddressBox = table.Column<byte>(nullable: false),
                    ScanQuantity = table.Column<short>(type: "smallint", nullable: false),
                    IsClientScanning = table.Column<bool>(type: "bit", nullable: false),
                    ReceptionActionId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceptionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceptionDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Appointments",
                columns: new[] { "Id", "Abbreviations", "InnerName", "OfficialName", "SalaryPerOneHour" },
                values: new object[,]
                {
                    { 1, "гр.;гр;груз;грузч.;", "Грузчик", "Грузчик", 47.85m },
                    { 2, "кл.;кладовщик;кл;клад;клад.;", "Кладовщик на РРЦ", "Кладовщик склада", 52.64m },
                    { 3, "пр.;приёмщик;приемщик;пр;", "Кладовщик приемщик", "Кладовщик-приемщик", 57.42m },
                    { 4, "вод.;водитель;вод;карщик;", "Водитель погрузчика", "Водитель погрузчика", 52.64m },
                    { 5, "ст.кл.;старший;ст;ст.;старшийкладовщик;ст.клад.;", "Старший кладовщик на РРЦ", "Старший кладовщик склада", 62.21m },
                    { 6, "зам.пр.;зампоприёмке;", "Заместитель управляющего склада по отгрузке", "Менеджер по отправке груза", 95.70m },
                    { 7, "зам.отгр.;зампоотгрузке;", "Заместитель управляющего склада по приемке", "Менеджер по приему груза", 92.22m },
                    { 8, "упр.скл.;управляющий;упр.;упр;упр.складом;", "Управляющий РРЦ", "Управляющий складом", 119.63m }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Categories",
                columns: new[] { "Id", "Abbreviation", "MaxVolume", "MinVolume", "Name" },
                values: new object[,]
                {
                    { 6, "кат.6", null, 250.0, "Товары от 250 литров" },
                    { 5, "кат.5", 250.0, 100.0, "Товары до 250 литров" },
                    { 4, "кат.4", 100.0, 25.0, "Товары до 100 литров" },
                    { 3, "кат.3", 25.0, 5.0, "Товары до 25 литров" },
                    { 2, "кат.2", 5.0, 2.5, "Товары до 5 литров" },
                    { 1, "кат.1", 2.5, null, "Товары до 2,5 литров" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "CategorySets",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Категории Владивостока" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Operations",
                columns: new[] { "Id", "Complexity", "Name", "OperationGroup" },
                values: new object[,]
                {
                    { 16, 1f, "Прочие операции", "Other" },
                    { 14, 1f, "Выгрузка машины", "Shipment" },
                    { 13, 1f, "Инвентаризация", "Inventory" },
                    { 12, 1f, "Упаковка товара в места", "Gathering" },
                    { 11, 1f, "Предварительный подбор товара", "Gathering" },
                    { 10, 1f, "Подбор товаров покупателей", "Gathering" },
                    { 9, 1f, "Подбор клиентского товара", "Gathering" },
                    { 15, 1f, "Погрузка машины", "Shipment" },
                    { 7, 1f, "Гор. дефрагментация", "Gathering" },
                    { 6, 1f, "Верт. дефрагментация", "Gathering" },
                    { 5, 1f, "Подтоварка", "Gathering" },
                    { 4, 1f, "Перемещение товара", "Gathering" },
                    { 3, 1f, "Размещение товара", "Gathering" },
                    { 2, 1f, "Сканирование транзитов", "Reception" },
                    { 1, 1f, "Сканирование товара", "Reception" },
                    { 8, 1f, "Подбор товара", "Gathering" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Positions",
                columns: new[] { "Id", "Abbreviations", "Complexity", "Name" },
                values: new object[,]
                {
                    { 11, "ст.см.кр.;ссмкр;", 1f, "Старший смены, крупняк" },
                    { 9, "рас.;расстановка;рас;", 1f, "Расстановка" },
                    { 8, "мез.4;мезонин4;мез4;", 1f, "Мезонин, 4-й этаж" },
                    { 7, "мез.3;мезонин3;мез3;", 1f, "Мезонин, 3-й этаж" },
                    { 6, "мез.2;мезонин2;мез2;", 1f, "Мезонин, 2-й этаж" },
                    { 10, "ст.см.мез.;ссммез;", 1f, "Старший смены, мезонин" },
                    { 4, "дор.;дор;", 1f, "Дорогая" },
                    { 3, "отгр.;отгр;от.;от;", 1f, "Отгрузка" },
                    { 2, "пр.;приемка;пр;", 1f, "Приёмка" },
                    { 1, "кр.;кр;", 1f, "Крупняк" },
                    { 5, "мез.1;мезонин1;мез1;", 1f, "Мезонин, 1-й этаж" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Ranks",
                columns: new[] { "Number", "OneHourCost" },
                values: new object[,]
                {
                    { 14, 533.33m },
                    { 13, 466.66m },
                    { 12, 442.42m },
                    { 11, 400m },
                    { 10, 366.66m },
                    { 9, 342.42m },
                    { 7, 266.67m },
                    { 6, 242.42m },
                    { 5, 220m },
                    { 4, 200m },
                    { 3, 180m },
                    { 2, 163m },
                    { 8, 300m }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Shifts",
                columns: new[] { "Id", "Duration", "Lunch", "Name", "RestTime", "StartTime" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 12, 0, 0, 0), new TimeSpan(0, 0, 30, 0, 0), "Дневная смена", new TimeSpan(0, 1, 0, 0, 0), new TimeSpan(0, 8, 0, 0, 0) },
                    { 2, new TimeSpan(0, 12, 0, 0, 0), new TimeSpan(0, 0, 30, 0, 0), "Ночная смена", new TimeSpan(0, 1, 0, 0, 0), new TimeSpan(0, 20, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "ShortBreaks",
                columns: new[] { "Id", "Duration", "FirstBreakTime", "Name", "Periodicity" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 0, 10, 0, 0), new TimeSpan(0, 9, 55, 0, 0), "Перекуры для некурящих", new TimeSpan(0, 2, 0, 0, 0) },
                    { 2, new TimeSpan(0, 0, 5, 0, 0), new TimeSpan(0, 8, 55, 0, 0), "Перекуры для курящих", new TimeSpan(0, 1, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "CategoryCategorySet",
                columns: new[] { "CategoryId", "CategorySetId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCategorySet_CategorySetId",
                table: "CategoryCategorySet",
                column: "CategorySetId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PositionId",
                schema: "dbo",
                table: "Addresses",
                column: "PositionId");

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
                name: "IX_DoubleAddressDetails_ProductId",
                schema: "dbo",
                table: "DoubleAddressDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressDetails_ReceiverAddressLetter_ReceiverAddressRow_ReceiverAddressSection_ReceiverAddressShelf_ReceiverAddressBox",
                schema: "dbo",
                table: "DoubleAddressDetails",
                columns: new[] { "ReceiverAddressLetter", "ReceiverAddressRow", "ReceiverAddressSection", "ReceiverAddressShelf", "ReceiverAddressBox" });

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressDetails_SenderAddressLetter_SenderAddressRow_SenderAddressSection_SenderAddressShelf_SenderAddressBox",
                schema: "dbo",
                table: "DoubleAddressDetails",
                columns: new[] { "SenderAddressLetter", "SenderAddressRow", "SenderAddressSection", "SenderAddressShelf", "SenderAddressBox" });

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
                name: "IX_InventoryDetails_ProductId",
                schema: "dbo",
                table: "InventoryDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_AddressLetter_AddressRow_AddressSection_AddressShelf_AddressBox",
                schema: "dbo",
                table: "InventoryDetails",
                columns: new[] { "AddressLetter", "AddressRow", "AddressSection", "AddressShelf", "AddressBox" });

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
                name: "IX_ReceptionDetails_ProductId",
                schema: "dbo",
                table: "ReceptionDetails",
                column: "ProductId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCategorySet");

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
                name: "ReceptionDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ShipmentActions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CategorySets",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DoubleAddressActions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryActions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ReceptionActions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Addresses",
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
