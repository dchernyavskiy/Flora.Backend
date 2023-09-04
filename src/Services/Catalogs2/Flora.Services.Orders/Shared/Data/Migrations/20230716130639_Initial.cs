using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flora.Services.Orders.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "order");

            migrationBuilder.CreateTable(
                name: "baskets",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customerid = table.Column<Guid>(name: "customer_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_baskets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    firstname = table.Column<string>(name: "first_name", type: "text", nullable: false),
                    lastname = table.Column<string>(name: "last_name", type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    customerid = table.Column<Guid>(name: "customer_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    imageurl = table.Column<string>(name: "image_url", type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    productstatus = table.Column<int>(name: "product_status", type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "basket_items",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    basketid = table.Column<Guid>(name: "basket_id", type: "uuid", nullable: false),
                    productid = table.Column<Guid>(name: "product_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_basket_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_basket_items_baskets_basket_id",
                        column: x => x.basketid,
                        principalSchema: "order",
                        principalTable: "baskets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_basket_items_products_product_id",
                        column: x => x.productid,
                        principalSchema: "order",
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    productid = table.Column<Guid>(name: "product_id", type: "uuid", nullable: false),
                    orderid = table.Column<Guid>(name: "order_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_items_orders_order_id",
                        column: x => x.orderid,
                        principalSchema: "order",
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_items_products_product_id",
                        column: x => x.productid,
                        principalSchema: "order",
                        principalTable: "products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_basket_items_basket_id",
                schema: "order",
                table: "basket_items",
                column: "basket_id");

            migrationBuilder.CreateIndex(
                name: "ix_basket_items_id",
                schema: "order",
                table: "basket_items",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_basket_items_product_id",
                schema: "order",
                table: "basket_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_baskets_id",
                schema: "order",
                table: "baskets",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_order_items_id",
                schema: "order",
                table: "order_items",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_order_items_order_id",
                schema: "order",
                table: "order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_items_product_id",
                schema: "order",
                table: "order_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_id",
                schema: "order",
                table: "orders",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_products_id",
                schema: "order",
                table: "products",
                column: "id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "basket_items",
                schema: "order");

            migrationBuilder.DropTable(
                name: "order_items",
                schema: "order");

            migrationBuilder.DropTable(
                name: "baskets",
                schema: "order");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "order");

            migrationBuilder.DropTable(
                name: "products",
                schema: "order");
        }
    }
}
