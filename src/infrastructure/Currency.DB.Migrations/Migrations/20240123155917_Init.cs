using System;
using Currency.DB;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Currency.DB.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:currency_type", "usd,eur,pln,uah");

            migrationBuilder.CreateTable(
                name: "currency_rates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_currency = table.Column<CurrencyType>(type: "currency_type", nullable: false),
                    to_currency = table.Column<CurrencyType>(type: "currency_type", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currency_rates", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "currency_rates");
        }
    }
}
