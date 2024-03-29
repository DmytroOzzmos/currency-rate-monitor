﻿// <auto-generated />
using System;
using Currency.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Currency.DB.Migrations.Migrations
{
    [DbContext(typeof(CurrencyDbContext))]
    partial class CurrencyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "currency_type", new[] { "usd", "eur", "pln", "uah" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Currency.DB.CurrencyRate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<CurrencyType>("FromCurrency")
                        .HasColumnType("currency_type")
                        .HasColumnName("from_currency");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("timestamp");

                    b.Property<CurrencyType>("ToCurrency")
                        .HasColumnType("currency_type")
                        .HasColumnName("to_currency");

                    b.HasKey("Id")
                        .HasName("pk_currency_rates");

                    b.ToTable("currency_rates", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
