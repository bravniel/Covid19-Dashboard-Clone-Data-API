using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoronaDataDashboard.API.Migrations
{
    public partial class addedModelsForTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<int>(type: "int", nullable: false),
                    RiskLevel = table.Column<int>(type: "int", nullable: false),
                    NumberOfEnteringPeopleToIsrael = table.Column<int>(type: "int", nullable: false),
                    NumberOfVerifiedCitizens = table.Column<int>(type: "int", nullable: false),
                    NumberOfVerifiedForeigners = table.Column<int>(type: "int", nullable: false),
                    PercentageOfVerifiedArrivals = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HospitalName = table.Column<int>(type: "int", nullable: false),
                    OverallBedOccupancyPercentage = table.Column<double>(type: "float", nullable: false),
                    PercentageOfIndoorWardBedOccupancy = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MorbidityFromAbroad",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MorbidityFromAbroad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settlements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SettlementName = table.Column<int>(type: "int", nullable: false),
                    ScoreAccordingToTrafficLightPlan = table.Column<double>(type: "float", nullable: false),
                    NewVerifiedPatients = table.Column<double>(type: "float", nullable: false),
                    PercentageOfPositiveTests = table.Column<double>(type: "float", nullable: false),
                    VerifiedChangeRate = table.Column<double>(type: "float", nullable: false),
                    ActivePatients = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settlements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CountryModelMorbidityFromAbroadModel",
                columns: table => new
                {
                    CountriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MorbidityFromAbroadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryModelMorbidityFromAbroadModel", x => new { x.CountriesId, x.MorbidityFromAbroadId });
                    table.ForeignKey(
                        name: "FK_CountryModelMorbidityFromAbroadModel_Countries_CountriesId",
                        column: x => x.CountriesId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryModelMorbidityFromAbroadModel_MorbidityFromAbroad_MorbidityFromAbroadId",
                        column: x => x.MorbidityFromAbroadId,
                        principalTable: "MorbidityFromAbroad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryModelMorbidityFromAbroadModel_MorbidityFromAbroadId",
                table: "CountryModelMorbidityFromAbroadModel",
                column: "MorbidityFromAbroadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryModelMorbidityFromAbroadModel");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "Settlements");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "MorbidityFromAbroad");
        }
    }
}
