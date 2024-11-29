using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "StockVision");

            migrationBuilder.CreateTable(
                name: "Assets",
                schema: "StockVision",
                columns: table => new
                {
                    AssetId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Symbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Exchange = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.AssetId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "StockVision",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "HistoricalData",
                schema: "StockVision",
                columns: table => new
                {
                    HistoricalDataId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClosingPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    MinPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Volume = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricalData", x => x.HistoricalDataId);
                    table.ForeignKey(
                        name: "FK_HistoricalData_Assets_AssetId",
                        column: x => x.AssetId,
                        principalSchema: "StockVision",
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Portfolios",
                schema: "StockVision",
                columns: table => new
                {
                    PortfolioId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PortfolioName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TotalValue = table.Column<decimal>(type: "numeric", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.PortfolioId);
                    table.ForeignKey(
                        name: "FK_Portfolios_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "StockVision",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "StockVision",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
                    AssetId = table.Column<int>(type: "integer", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "numeric", nullable: false),
                    TransactionType = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_Assets_AssetId",
                        column: x => x.AssetId,
                        principalSchema: "StockVision",
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalSchema: "StockVision",
                        principalTable: "Portfolios",
                        principalColumn: "PortfolioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Symbol",
                schema: "StockVision",
                table: "Assets",
                column: "Symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricalData_AssetId",
                schema: "StockVision",
                table: "HistoricalData",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_UserId",
                schema: "StockVision",
                table: "Portfolios",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AssetId",
                schema: "StockVision",
                table: "Transactions",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PortfolioId",
                schema: "StockVision",
                table: "Transactions",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "StockVision",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricalData",
                schema: "StockVision");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "StockVision");

            migrationBuilder.DropTable(
                name: "Assets",
                schema: "StockVision");

            migrationBuilder.DropTable(
                name: "Portfolios",
                schema: "StockVision");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "StockVision");
        }
    }
}
