using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flora.Services.Catalogs.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    imageurl = table.Column<string>(name: "image_url", type: "text", nullable: true),
                    ismain = table.Column<bool>(name: "is_main", type: "boolean", nullable: true),
                    parentid = table.Column<Guid>(name: "parent_id", type: "uuid", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                    table.ForeignKey(
                        name: "fk_categories_categories_parent_id",
                        column: x => x.parentid,
                        principalSchema: "catalog",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "characteristics",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    categoryid = table.Column<Guid>(name: "category_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_characteristics", x => x.id);
                    table.ForeignKey(
                        name: "fk_characteristics_categories_category_id",
                        column: x => x.categoryid,
                        principalSchema: "catalog",
                        principalTable: "categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "products",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    productstatus = table.Column<string>(name: "product_status", type: "character varying(25)", maxLength: 25, nullable: false, defaultValue: "Available"),
                    categoryid = table.Column<Guid>(name: "category_id", type: "uuid", nullable: false),
                    stockavailable = table.Column<int>(name: "stock_available", type: "integer", nullable: false),
                    stockrestockthreshold = table.Column<int>(name: "stock_restock_threshold", type: "integer", nullable: false),
                    stockmaxstockthreshold = table.Column<int>(name: "stock_max_stock_threshold", type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_categories_category_id",
                        column: x => x.categoryid,
                        principalSchema: "catalog",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "characteristic_values",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    productid = table.Column<Guid>(name: "product_id", type: "uuid", nullable: false),
                    characteristicid = table.Column<Guid>(name: "characteristic_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_characteristic_values", x => x.id);
                    table.ForeignKey(
                        name: "fk_characteristic_values_characteristics_characteristic_id",
                        column: x => x.characteristicid,
                        principalSchema: "catalog",
                        principalTable: "characteristics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_characteristic_values_products_product_id",
                        column: x => x.productid,
                        principalSchema: "catalog",
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "products_images",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    imageurl = table.Column<string>(name: "image_url", type: "text", nullable: false),
                    ismain = table.Column<bool>(name: "is_main", type: "boolean", nullable: false),
                    ownerid = table.Column<Guid>(name: "owner_id", type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_images_products_product_id",
                        column: x => x.ownerid,
                        principalSchema: "catalog",
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_categories_id",
                schema: "catalog",
                table: "categories",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_categories_parent_id",
                schema: "catalog",
                table: "categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_characteristic_values_characteristic_id",
                schema: "catalog",
                table: "characteristic_values",
                column: "characteristic_id");

            migrationBuilder.CreateIndex(
                name: "ix_characteristic_values_id",
                schema: "catalog",
                table: "characteristic_values",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_characteristic_values_product_id",
                schema: "catalog",
                table: "characteristic_values",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_characteristics_category_id",
                schema: "catalog",
                table: "characteristics",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_characteristics_id",
                schema: "catalog",
                table: "characteristics",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_products_category_id",
                schema: "catalog",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_id",
                schema: "catalog",
                table: "products",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_products_images_id",
                schema: "catalog",
                table: "products_images",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_products_images_owner_id",
                schema: "catalog",
                table: "products_images",
                column: "owner_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "characteristic_values",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "products_images",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "characteristics",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "products",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "catalog");
        }
    }
}
