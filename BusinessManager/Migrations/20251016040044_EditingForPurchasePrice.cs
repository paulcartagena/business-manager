using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessManager.Migrations
{
    /// <inheritdoc />
    public partial class EditingForPurchasePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "purchase_price",
                table: "product");

            migrationBuilder.AddColumn<decimal>(
                name: "total_amount",
                table: "purchase",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "purchase");

            migrationBuilder.AddColumn<decimal>(
                name: "purchase_price",
                table: "product",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
