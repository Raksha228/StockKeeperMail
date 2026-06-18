using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockKeeperMail.Database.Migrations
{
    /// <inheritdoc />
    public partial class EnsureOrderDeliveryFieldsAndNumbers : Migration
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

;WITH NumberedOrders AS
(
    SELECT
        [OrderID],
        ROW_NUMBER() OVER (ORDER BY [OrderDate], [OrderID]) AS [RowNumber]
    FROM [dbo].[Orders]
)
UPDATE o
SET
    [ExternalOrderNumber] = COALESCE(
        NULLIF(LTRIM(RTRIM(o.[ExternalOrderNumber])), N''),
        CONCAT(N'EXT-', CONVERT(char(8), o.[OrderDate], 112), N'-', RIGHT(N'0000' + CONVERT(nvarchar(10), n.[RowNumber]), 4))
    ),
    [DeliveryAddress] = COALESCE(
        NULLIF(LTRIM(RTRIM(o.[DeliveryAddress])), N''),
        c.[CustomerAddress]
    ),
    [IsOnlineOrder] = CASE
        WHEN NULLIF(LTRIM(RTRIM(COALESCE(o.[ExternalOrderNumber], N''))), N'') IS NULL THEN 1
        ELSE o.[IsOnlineOrder]
    END
FROM [dbo].[Orders] o
INNER JOIN NumberedOrders n ON n.[OrderID] = o.[OrderID]
LEFT JOIN [dbo].[Customers] c ON c.[CustomerID] = o.[CustomerID];
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE [dbo].[Orders]
SET
    [ExternalOrderNumber] = NULL,
    [DeliveryAddress] = NULL,
    [IsOnlineOrder] = 0
WHERE COL_LENGTH('dbo.Orders', 'ExternalOrderNumber') IS NOT NULL
  AND COL_LENGTH('dbo.Orders', 'DeliveryAddress') IS NOT NULL
  AND COL_LENGTH('dbo.Orders', 'IsOnlineOrder') IS NOT NULL;
");
        }
    }
}
