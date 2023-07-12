﻿// <auto-generated />
using System;
using Flora.Services.Orders.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Flora.Services.Orders.Shared.Data.Migrations
{
    [DbContext(typeof(OrdersDbContext))]
    [Migration("20230712190714_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Flora.Services.Orders.Orders.Models.Order", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("integer")
                        .HasColumnName("created_by");

                    b.Property<long>("OriginalVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("bigint")
                        .HasColumnName("original_version");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_orders_id");

                    b.ToTable("orders", "order");
                });

            modelBuilder.Entity("Flora.Services.Orders.Orders.Models.Order", b =>
                {
                    b.OwnsOne("Flora.Services.Orders.Orders.ValueObjects.CustomerInfo", "Customer", b1 =>
                        {
                            b1.Property<long>("OrderId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<long>("CustomerId")
                                .HasColumnType("bigint")
                                .HasColumnName("customer_customer_id");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("customer_name");

                            b1.HasKey("OrderId");

                            b1.ToTable("orders", "order");

                            b1.WithOwner()
                                .HasForeignKey("OrderId")
                                .HasConstraintName("fk_orders_orders_id");
                        });

                    b.OwnsOne("Flora.Services.Orders.Orders.ValueObjects.ProductInfo", "Product", b1 =>
                        {
                            b1.Property<long>("OrderId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("product_name");

                            b1.Property<decimal>("Price")
                                .HasColumnType("numeric")
                                .HasColumnName("product_price");

                            b1.Property<long>("ProductId")
                                .HasColumnType("bigint")
                                .HasColumnName("product_product_id");

                            b1.HasKey("OrderId");

                            b1.ToTable("orders", "order");

                            b1.WithOwner()
                                .HasForeignKey("OrderId")
                                .HasConstraintName("fk_orders_orders_id");
                        });

                    b.Navigation("Customer")
                        .IsRequired();

                    b.Navigation("Product")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
