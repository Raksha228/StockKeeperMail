using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockKeeperMail.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddMasterFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Orders', 'ExternalOrderNumber') IS NULL
    ALTER TABLE [dbo].[Orders] ADD [ExternalOrderNumber] nvarchar(max) NULL;

IF COL_LENGTH('dbo.Orders', 'IsOnlineOrder') IS NULL
    ALTER TABLE [dbo].[Orders] ADD [IsOnlineOrder] bit NOT NULL CONSTRAINT [DF_Orders_IsOnlineOrder] DEFAULT(0);

IF COL_LENGTH('dbo.Orders', 'DeliveryAddress') IS NULL
    ALTER TABLE [dbo].[Orders] ADD [DeliveryAddress] nvarchar(max) NULL;
");

            migrationBuilder.CreateTable(
                name: "PurchaseReceipts",
                columns: table => new
                {
                    PurchaseReceiptID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PurchasedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseReceipts", x => x.PurchaseReceiptID);
                    table.ForeignKey(
                        name: "FK_PurchaseReceipts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseReceipts_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseReceipts_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReceipts_ProductID",
                table: "PurchaseReceipts",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReceipts_SupplierID",
                table: "PurchaseReceipts",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReceipts_WarehouseID",
                table: "PurchaseReceipts",
                column: "WarehouseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseReceipts");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Orders', 'ExternalOrderNumber') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP COLUMN [ExternalOrderNumber];

IF COL_LENGTH('dbo.Orders', 'IsOnlineOrder') IS NOT NULL
BEGIN
    DECLARE @constraintName nvarchar(128);
    SELECT @constraintName = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    INNER JOIN sys.tables t ON t.object_id = c.object_id
    WHERE t.name = 'Orders' AND c.name = 'IsOnlineOrder';

    IF @constraintName IS NOT NULL
        EXEC('ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [' + @constraintName + ']');

    ALTER TABLE [dbo].[Orders] DROP COLUMN [IsOnlineOrder];
END

IF COL_LENGTH('dbo.Orders', 'DeliveryAddress') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP COLUMN [DeliveryAddress];
");
        }
    }
}
