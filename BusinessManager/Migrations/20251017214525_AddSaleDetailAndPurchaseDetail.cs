using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessManager.Migrations
{
    /// <inheritdoc />
    public partial class AddSaleDetailAndPurchaseDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "sale",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "purchase",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "movement_inventory",
                newName: "created_at");

            migrationBuilder.AddColumn<decimal>(
                name: "total_amount",
                table: "sale",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "purchase_detail",
                columns: table => new
                {
                    purchase_detail_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    purchase_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    sub_total = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase_detail", x => x.purchase_detail_id);
                    table.ForeignKey(
                        name: "FK_purchase_detail_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_purchase_detail_purchase_purchase_id",
                        column: x => x.purchase_id,
                        principalTable: "purchase",
                        principalColumn: "purchase_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sale_detail",
                columns: table => new
                {
                    sale_detail_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sale_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    sub_total = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sale_detail", x => x.sale_detail_id);
                    table.ForeignKey(
                        name: "FK_sale_detail_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sale_detail_sale_sale_id",
                        column: x => x.sale_id,
                        principalTable: "sale",
                        principalColumn: "sale_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_detail_product_id",
                table: "purchase_detail",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_detail_purchase_id",
                table: "purchase_detail",
                column: "purchase_id");

            migrationBuilder.CreateIndex(
                name: "IX_sale_detail_product_id",
                table: "sale_detail",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_sale_detail_sale_id",
                table: "sale_detail",
                column: "sale_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "purchase_detail");

            migrationBuilder.DropTable(
                name: "sale_detail");

            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "sale");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "sale",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "purchase",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "movement_inventory",
                newName: "created_date");
        }
    }
}
