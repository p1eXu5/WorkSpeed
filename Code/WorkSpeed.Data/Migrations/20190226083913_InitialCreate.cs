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
                    OfficialName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    InnerName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    SalaryPerOneHour = table.Column<decimal>(type: "decimal", nullable: true),
                    Abbreviations = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Avatars",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Stride = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(16)", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: true)
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
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Group = table.Column<string>(type: "varchar(10)", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Complexity = table.Column<float>(type: "real", nullable: true),
                    Abbreviations = table.Column<string>(type: "nvarchar(50)", nullable: false)
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
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 196, nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(20)", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(25)", nullable: false),
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
                    Letter = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
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
                    Id = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSmoker = table.Column<bool>(type: "bit", nullable: true),
                    ProbationEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvatarId = table.Column<int>(nullable: true),
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
                        name: "FK_Employees_Avatars_AvatarId",
                        column: x => x.AvatarId,
                        principalSchema: "dbo",
                        principalTable: "Avatars",
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
                    Id = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    EmployeeId = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false)
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
                    Id = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    EmployeeId = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false)
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
                    Id = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    EmployeeId = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false)
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
                    Id = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    EmployeeId = table.Column<string>(nullable: false),
                    OperationId = table.Column<int>(nullable: false)
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
                    Id = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(7)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    OperationId = table.Column<int>(nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: true),
                    Volume = table.Column<float>(type: "real", nullable: true),
                    ClientCargoQuantity = table.Column<float>(type: "real", nullable: true),
                    CommonCargoQuantity = table.Column<float>(type: "real", nullable: true)
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
                        name: "FK_ShipmentActions_Operations_OperationId",
                        column: x => x.OperationId,
                        principalSchema: "dbo",
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoubleAddressActionDetails",
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
                    DoubleAddressActionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoubleAddressActionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoubleAddressActionDetails_DoubleAddressActions_DoubleAddressActionId",
                        column: x => x.DoubleAddressActionId,
                        principalSchema: "dbo",
                        principalTable: "DoubleAddressActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoubleAddressActionDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoubleAddressActionDetails_Addresses_ReceiverAddressLetter_ReceiverAddressRow_ReceiverAddressSection_ReceiverAddressShelf_Re~",
                        columns: x => new { x.ReceiverAddressLetter, x.ReceiverAddressRow, x.ReceiverAddressSection, x.ReceiverAddressShelf, x.ReceiverAddressBox },
                        principalSchema: "dbo",
                        principalTable: "Addresses",
                        principalColumns: new[] { "Letter", "Row", "Section", "Shelf", "Box" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoubleAddressActionDetails_Addresses_SenderAddressLetter_SenderAddressRow_SenderAddressSection_SenderAddressShelf_SenderAddr~",
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
                    { 6, "зам.пр.;зампоприёмке;зампр;", "Заместитель управляющего склада по отгрузке", "Менеджер по отправке груза", 95.70m },
                    { 7, "зам.отгр.;зампоотгрузке;замотгр;", "Заместитель управляющего склада по приемке", "Менеджер по приему груза", 92.22m },
                    { 8, "упр.скл.;управляющий;упр.;упр;упр.складом;", "Управляющий РРЦ", "Управляющий складом", 119.63m }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Avatars",
                columns: new[] { "Id", "Height", "Picture", "Stride", "Width" },
                values: new object[] { 1, 65, new byte[] { 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 233, 233, 233, 233, 233, 233, 231, 231, 231, 234, 234, 234, 233, 233, 233, 233, 233, 233, 233, 233, 233, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 216, 216, 216, 147, 147, 147, 96, 96, 96, 95, 95, 95, 92, 92, 92, 145, 145, 145, 219, 219, 219, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 217, 217, 217, 128, 128, 128, 61, 61, 61, 54, 54, 54, 61, 61, 61, 61, 61, 61, 64, 64, 64, 66, 66, 66, 78, 78, 78, 162, 162, 162, 234, 234, 234, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 234, 234, 234, 143, 143, 143, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 89, 89, 89, 202, 202, 202, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 199, 199, 199, 88, 88, 88, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 146, 146, 146, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 138, 138, 138, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 105, 105, 105, 227, 227, 227, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 124, 124, 124, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 82, 82, 82, 224, 224, 224, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 163, 163, 163, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 78, 78, 78, 226, 226, 226, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 226, 226, 226, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 99, 99, 99, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 140, 140, 140, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 185, 185, 185, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 233, 233, 233, 67, 67, 67, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 99, 99, 99, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 194, 194, 194, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 61, 61, 61, 222, 222, 222, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 136, 136, 136, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 164, 164, 164, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 107, 107, 107, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 103, 103, 103, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 234, 234, 234, 75, 75, 75, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 76, 76, 76, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 233, 233, 233, 72, 72, 72, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 65, 65, 65, 234, 234, 234, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 234, 234, 234, 72, 72, 72, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 68, 68, 68, 233, 233, 233, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 104, 104, 104, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 69, 69, 69, 234, 234, 234, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 139, 139, 139, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 61, 61, 61, 100, 100, 100, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 135, 135, 135, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 63, 63, 63, 139, 139, 139, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 228, 228, 228, 105, 105, 105, 105, 105, 105, 70, 70, 70, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 62, 62, 62, 126, 126, 126, 226, 226, 226, 234, 234, 234, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 223, 223, 223, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 63, 63, 63, 226, 226, 226, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 230, 230, 230, 70, 70, 70, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 227, 227, 227, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 100, 100, 100, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 76, 76, 76, 234, 234, 234, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 135, 135, 135, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 135, 135, 135, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 215, 215, 215, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 65, 65, 65, 224, 224, 224, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 91, 91, 91, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 108, 108, 108, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 155, 155, 155, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 163, 163, 163, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 226, 226, 226, 73, 73, 73, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 215, 215, 215, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 212, 212, 212, 63, 63, 63, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 61, 61, 61, 181, 181, 181, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 113, 113, 113, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 122, 122, 122, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 155, 155, 155, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 190, 190, 190, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 211, 211, 211, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 71, 71, 71, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 144, 144, 144, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 68, 68, 68, 225, 225, 225, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 231, 231, 231, 98, 98, 98, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 83, 83, 83, 215, 215, 215, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 231, 231, 231, 87, 87, 87, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 61, 61, 61, 188, 188, 188, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 123, 123, 123, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 204, 204, 204, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 232, 232, 232, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 90, 90, 90, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 211, 211, 211, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 221, 221, 221, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 181, 181, 181, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 190, 190, 190, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 176, 176, 176, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 185, 185, 185, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 170, 170, 170, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 187, 187, 187, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 134, 134, 134, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 186, 186, 186, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 137, 137, 137, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 192, 192, 192, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 180, 180, 180, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 188, 188, 188, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 218, 218, 218, 107, 107, 107, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 92, 92, 92, 204, 204, 204, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 142, 142, 142, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 92, 92, 92, 179, 179, 179, 232, 232, 232, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 235, 203, 203, 203, 91, 91, 91, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 78, 78, 78, 152, 152, 152, 216, 216, 216, 235, 235, 235, 235, 235, 235, 236, 236, 236, 0, 0, 236, 236, 236, 231, 231, 231, 202, 202, 202, 157, 157, 157, 102, 102, 102, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 117, 117, 117, 197, 197, 197, 235, 235, 235, 0, 0, 142, 142, 142, 73, 73, 73, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 94, 94, 94, 0, 0, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 65, 65, 65, 0, 0, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 0, 0, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 0, 0, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 0, 0, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 0, 0, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 0, 0, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 0, 0, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 0, 0, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 0, 0, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 0, 0, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 0, 0, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 0, 0 }, 152, 50 });

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
                columns: new[] { "Id", "Complexity", "Group", "Name" },
                values: new object[,]
                {
                    { 16, 1f, "Other", "Прочие операции" },
                    { 14, 1f, "Shipment", "Выгрузка машины" },
                    { 13, 1f, "Inventory", "Инвентаризация" },
                    { 12, 1f, "Placing", "Упаковка товара в места" },
                    { 10, 1f, "BuyerGathering", "Подбор товаров покупателей" },
                    { 11, 1f, "Gathering", "Предварительный подбор товара" },
                    { 9, 1f, "Gathering", "Подбор клиентского товара" },
                    { 15, 1f, "Shipment", "Погрузка машины" },
                    { 7, 1f, "Defragmentation", "Гор. дефрагментация" },
                    { 6, 1f, "Defragmentation", "Верт. дефрагментация" },
                    { 5, 1f, "Defragmentation", "Подтоварка" },
                    { 4, 1f, "Placing", "Перемещение товара" },
                    { 3, 1f, "Placing", "Размещение товара" },
                    { 2, 1f, "Reception", "Сканирование транзитов" },
                    { 1, 1f, "Reception", "Сканирование товара" },
                    { 8, 1f, "Gathering", "Подбор товара" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Positions",
                columns: new[] { "Id", "Abbreviations", "Complexity", "Name" },
                values: new object[,]
                {
                    { 13, "дор.;дор;", 1f, "Дорогая" },
                    { 12, "рас.;расстановка;рас;", 1f, "Расстановка" },
                    { 10, "мез.3;мезонин3;мез3;", 1f, "Мезонин, 3-й этаж" },
                    { 9, "мез.2;мезонин2;мез2;", 1f, "Мезонин, 2-й этаж" },
                    { 8, "мез.1;мезонин1;мез1;", 1f, "Мезонин, 1-й этаж" },
                    { 7, "ст.см.мез.;ссммез;", 1f, "Старший смены, мезонин" },
                    { 11, "мез.4;мезонин4;мез4;", 1f, "Мезонин, 4-й этаж" },
                    { 5, "ст.см.кр.;ссмкр;", 1f, "Старший смены, крупняк" },
                    { 4, "вод.;водитель;вод;", 1f, "Водитель" },
                    { 3, "отгр.;отгр;от.;от;", 1f, "Отгрузка" },
                    { 2, "пр.;приемка;пр;", 1f, "Приёмка" },
                    { 1, "упр.;упр;склад;", 1f, "Склад" },
                    { 6, "кр.;кр;", 1f, "Крупняк" }
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
                name: "IX_DoubleAddressActionDetails_DoubleAddressActionId",
                schema: "dbo",
                table: "DoubleAddressActionDetails",
                column: "DoubleAddressActionId");

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressActionDetails_ProductId",
                schema: "dbo",
                table: "DoubleAddressActionDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressActionDetails_ReceiverAddressLetter_ReceiverAddressRow_ReceiverAddressSection_ReceiverAddressShelf_ReceiverAddr~",
                schema: "dbo",
                table: "DoubleAddressActionDetails",
                columns: new[] { "ReceiverAddressLetter", "ReceiverAddressRow", "ReceiverAddressSection", "ReceiverAddressShelf", "ReceiverAddressBox" });

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressActionDetails_SenderAddressLetter_SenderAddressRow_SenderAddressSection_SenderAddressShelf_SenderAddressBox",
                schema: "dbo",
                table: "DoubleAddressActionDetails",
                columns: new[] { "SenderAddressLetter", "SenderAddressRow", "SenderAddressSection", "SenderAddressShelf", "SenderAddressBox" });

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressActions_EmployeeId",
                schema: "dbo",
                table: "DoubleAddressActions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DoubleAddressActions_OperationId",
                schema: "dbo",
                table: "DoubleAddressActions",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AppointmentId",
                schema: "dbo",
                table: "Employees",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AvatarId",
                schema: "dbo",
                table: "Employees",
                column: "AvatarId");

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
                name: "DoubleAddressActionDetails",
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
                name: "Avatars",
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
