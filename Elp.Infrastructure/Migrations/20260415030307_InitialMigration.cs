using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Elp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Codebooks",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameCz = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codebooks", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "DriverFitnessCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicalProfessionalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverFitnessCertificates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodebookItems",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodebookCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DescriptionCz = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodebookItems", x => new { x.CodebookCode, x.Code });
                    table.ForeignKey(
                        name: "FK_CodebookItems_Codebooks_CodebookCode",
                        column: x => x.CodebookCode,
                        principalTable: "Codebooks",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FitnessGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverFitnessCertificateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverGroupCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitnessGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FitnessGroups_DriverFitnessCertificates_DriverFitnessCertificateId",
                        column: x => x.DriverFitnessCertificateId,
                        principalTable: "DriverFitnessCertificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Codebooks",
                columns: new[] { "Code", "NameCz", "NameEn" },
                values: new object[,]
                {
                    { "StavPosudku", "Stavy lékařských posudků", "Medical Certificate Statuses" },
                    { "TypOpravneni", "Typy řidičských oprávnění", "Driving License Types" }
                });

            migrationBuilder.InsertData(
                table: "DriverFitnessCertificates",
                columns: new[] { "Id", "ExpirationDate", "IssueDate", "MedicalProfessionalId", "PersonalId", "StatusCode" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), null, new DateTime(2025, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111"), "ABC-123-456", "VYDANO" },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), null, new DateTime(2026, 2, 10, 9, 30, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111"), "XYZ-987-654", "ZAMITNUTO" },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), null, new DateTime(2020, 5, 20, 14, 15, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222"), "ABC-123-456", "ZNEPLATNENO" },
                    { new Guid("d4444444-4444-4444-4444-444444444444"), null, new DateTime(2026, 4, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222"), "LMN-456-789", "VYDANO" }
                });

            migrationBuilder.InsertData(
                table: "CodebookItems",
                columns: new[] { "Code", "CodebookCode", "DescriptionCz", "DescriptionEn", "IsActive" },
                values: new object[,]
                {
                    { "VYDANO", "StavPosudku", "Posudek byl vydán", "Certificate Issued", true },
                    { "ZAMITNUTO", "StavPosudku", "Posudek byl zamítnut", "Certificate Rejected", true },
                    { "ZNEPLATNENO", "StavPosudku", "Posudek byl zneplatněn", "Certificate Invalidated", true },
                    { "A", "TypOpravneni", "Motocykly", "Motorcycles", true },
                    { "B", "TypOpravneni", "Osobní automobily", "Passenger cars", true },
                    { "C", "TypOpravneni", "Nákladní automobily", "Trucks", true },
                    { "D", "TypOpravneni", "Autobusy", "Buses", true },
                    { "T", "TypOpravneni", "Traktory", "Tractors", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FitnessGroups_DriverFitnessCertificateId",
                table: "FitnessGroups",
                column: "DriverFitnessCertificateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodebookItems");

            migrationBuilder.DropTable(
                name: "FitnessGroups");

            migrationBuilder.DropTable(
                name: "Codebooks");

            migrationBuilder.DropTable(
                name: "DriverFitnessCertificates");
        }
    }
}
