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
                    { "akce_ro", "akce ro", "event" },
                    { "druh_posudku_ro", "druh posudku ro", "assessment type" },
                    { "druh_prohlidky_ro", "druh prohlidky ro", "type of inspection" },
                    { "seznam_harmonizovane_kody_ro", "harmonizovane kody", "harmonized codes" },
                    { "seznam_narodni_kody_ro", "harmonizovane kody", "country codes" },
                    { "seznam_skupin_ro", "seznam skupin", "vehicle type" },
                    { "skupina_ro", "skupina ro", "group" },
                    { "stav_posudku", "stav posudku", "assessment status" },
                    { "vysledek_posudku_ro", "vysledek posudku ro", "assessment result" }
                });

            migrationBuilder.InsertData(
                table: "DriverFitnessCertificates",
                columns: new[] { "Id", "ExpirationDate", "IssueDate", "MedicalProfessionalId", "PersonalId", "StatusCode" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), null, new DateTime(2025, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111"), "ABC-123-456", "stav_posudku_1" },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), null, new DateTime(2026, 2, 10, 9, 30, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111"), "XYZ-987-654", "stav_posudku_3" },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), null, new DateTime(2020, 5, 20, 14, 15, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222"), "ABC-123-456", "stav_posudku_2" },
                    { new Guid("d4444444-4444-4444-4444-444444444444"), null, new DateTime(2026, 4, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222"), "LMN-456-789", "stav_posudku_1" }
                });

            migrationBuilder.InsertData(
                table: "CodebookItems",
                columns: new[] { "Code", "CodebookCode", "DescriptionCz", "DescriptionEn", "IsActive" },
                values: new object[,]
                {
                    { "akce_ro_1", "akce_ro", "vytvoření", "creation", true },
                    { "akce_ro_2", "akce_ro", "aktualizace", "update", true },
                    { "akce_ro_3", "akce_ro", "zneplatnění", "invalidation", true },
                    { "druh_posudku_ro_1", "druh_posudku_ro", "prvořidič", "lead driver", true },
                    { "druh_posudku_ro_2", "druh_posudku_ro", "rozšíření řidičského oprávnění", "Driver's license extension", true },
                    { "druh_posudku_ro_3", "druh_posudku_ro", "prodloužení řidičského oprávnění", "renewal of a driver's license", true },
                    { "druh_posudku_ro_4", "druh_posudku_ro", "senioři", "seniors", true },
                    { "druh_posudku_ro_5", "druh_posudku_ro", "přezkoumání způsobilosti", "eligibility review", true },
                    { "druh_prohlidky_ro1", "druh_prohlidky_ro", "vstupní", "input", true },
                    { "druh_prohlidky_ro2", "druh_prohlidky_ro", "pravidelná", "regular", true },
                    { "druh_prohlidky_ro3", "druh_prohlidky_ro", "mimořádná", "extraordinary", true },
                    { "10.02", "seznam_harmonizovane_kody_ro", "10.02 Automatická převodovka", "10.02 Automatic transmission", true },
                    { "10.04", "seznam_harmonizovane_kody_ro", "10.04 Přizpůsobené ústrojí ovládání převodovky", "10.04 Adapted transmission control mechanism", true },
                    { "20.03", "seznam_harmonizovane_kody_ro", "20.03 Brzdový pedál upravený na levou nohu", "20.03 Brake pedal adapted for the left foot", true },
                    { "20.07", "seznam_harmonizovane_kody_ro", "20.07 Ovládání brzdy s použitím maximální síly ... N [například: 20.07(300 N)]", "20.07 Brake operation using a maximum force of ... N [for example: 20.07(300 N)]", true },
                    { "25.01", "seznam_harmonizovane_kody_ro", "25.01 Přizpůsobený pedál akcelerátoru", "25.01 Adapted accelerator pedal", true },
                    { "25.05", "seznam_harmonizovane_kody_ro", "25.05 Ovládání akcelerátoru kolenem", "25.05 Knee-operated accelerator", true },
                    { "25.09", "seznam_harmonizovane_kody_ro", "25.09 Opatření proti zablokování nebo aktivaci akcelerátoru", "25.09 Measures against blocking or activating the accelerator", true },
                    { "31.04", "seznam_harmonizovane_kody_ro", "31.04 Zvýšená podlaha", "31.04 Raised floor", true },
                    { "35.05", "seznam_harmonizovane_kody_ro", "35.05 Ovladače ovladatelné bez puštění zařízení pro řízení vozidla a mechanismy akcelerátoru a brzd", "35.05 Controls operable without releasing the steering device and accelerator and brake mechanisms", true },
                    { "40.05", "seznam_harmonizovane_kody_ro", "40.05 Přizpůsobený volant (větší nebo silnější volant, zmenšený průměr volantu, apod.)", "40.05 Adapted steering wheel (larger or thicker steering wheel, reduced steering wheel diameter, etc.)", true },
                    { "42.01", "seznam_harmonizovane_kody_ro", "42.01 Přizpůsobené zařízení pro výhled dozadu", "42.01 Adapted rear-view device", true },
                    { "42.05", "seznam_harmonizovane_kody_ro", "42.05 Zařízení k eliminaci mrtvého úhlu", "42.05 Device to eliminate blind spots", true },
                    { "43.01", "seznam_harmonizovane_kody_ro", "43.01 Výška sedadla řidiče umožňující normální výhled a v normální vzdálenosti od  volantu a pedálů", "43.01 Driver's seat height allowing normal view and at a normal distance from the steering wheel and pedals", true },
                    { "44.02", "seznam_harmonizovane_kody_ro", "44.02 Přizpůsobená brzda na předním kole", "44.02 Adapted front wheel brake", true },
                    { "44.10", "seznam_harmonizovane_kody_ro", "44.10 Maximální ovládací síla brzdy zadního kola... N [například 44.10 (240 N)]", "44.10 Maximum operating force of the rear wheel brake... N [for example 44.10 (240 N)]", true },
                    { "61", "seznam_harmonizovane_kody_ro", "61. Omezení jízdy podle denní doby (například: jedna hodina po východu slunce a jedna hodina před západem slunce)", "61. Driving restriction by time of day (for example: one hour after sunrise and one hour before sunset)", true },
                    { "62", "seznam_harmonizovane_kody_ro", "62. Omezení jízdy v okruhu ... km od místa bydliště řidiče nebo pouze ve městě/regionu", "62. Driving restriction within a radius of ... km from the driver's place of residence or only within a city/region", true },
                    { "63", "seznam_harmonizovane_kody_ro", "63. Řízení vozidla bez cestujících", "63. Driving a vehicle without passengers", true },
                    { "64", "seznam_harmonizovane_kody_ro", "64. Jízda rychlostí nepřesahující ... km/h", "64. Driving at a speed not exceeding ... km/h", true },
                    { "67", "seznam_harmonizovane_kody_ro", "67. Zákaz jízdy na dálnici", "67. Driving on motorways prohibited", true },
                    { "105", "seznam_narodni_kody_ro", "105. Jiná zdravotní omezení, která nejsou uvedena v části I této přílohy", "105. Other health restrictions not listed in Part I of this Annex", true },
                    { "111a", "seznam_narodni_kody_ro", "111. Nelze vykonávat činnost: a) řidiče, který řídí motorové vozidlo v pracovněprávním vztahu a u něhož je řízení motorového vozidla druhem práce sjednaným v pracovní smlouvě", "111. Cannot perform the activity of: a) a driver who drives a motor vehicle in an employment relationship and for whom driving a motor vehicle is the type of work agreed upon in the employment contract", true },
                    { "111b", "seznam_narodni_kody_ro", "111. Nelze vykonávat činnost: b) řidiče, u kterého je řízení motorového vozidla předmětem samostatné výdělečné činnosti prováděné podle jiného právního předpisu", "111. Cannot perform the activity of: b) a driver for whom driving a motor vehicle is the subject of self-employment carried out under another legal regulation", true },
                    { "111c", "seznam_narodni_kody_ro", "111. Nelze vykonávat činnost: c) učitele výcviku v řízení motorových vozidel podle jiného právního předpisu", "111. Cannot perform the activity of: c) a driving school instructor under another legal regulation", true },
                    { "115", "seznam_narodni_kody_ro", "115. Posilovač spojky", "115. Power clutch", true },
                    { "160", "seznam_narodni_kody_ro", "160. Výjimka z věku u řidičského oprávnění skupiny vozidel A1, A2 nebo A osoby, která je držitelem licence motoristického sportovce, udělená pouze pro jízdu při sportovní soutěži", "160. Age exemption for a driving license of vehicle category A1, A2 or A for a person holding a motorsport athlete license, granted only for driving during a sports competition", true },
                    { "172", "seznam_narodni_kody_ro", "172. Omezení řidičského oprávnění skupiny vozidel A pouze k řízení motorového vozíku pro invalidy", "172. Restriction of the driving license for vehicle category A only to driving a motorized wheelchair for the disabled", true },
                    { "175", "seznam_narodni_kody_ro", "175. Omezení řidičského oprávnění skupiny vozidel D pouze k řízení vozidla městské hromadné dopravy osob", "175. Restriction of the driving license for vehicle category D only to driving a public transport vehicle", true },
                    { "185", "seznam_narodni_kody_ro", "185. Pouze pro řízení motorových vozidel stanovených v § 83 odst. 5 zákona č. 361/2000 Sb. do doby dosažení věku u skupiny vozidel C 21 let, u skupiny vozidel D 24 let", "185. Only for driving motor vehicles specified in Section 83, Paragraph 5 of Act No. 361/2000 Coll. until reaching the age of 21 for vehicle category C, and 24 for vehicle category D", true },
                    { "199", "seznam_narodni_kody_ro", "199. Zkušební doba podmíněného upuštění od výkon", "199. Probationary period of conditional waiver of execution", true },
                    { "A", "seznam_skupin_ro", "skupina A", "Group A", true },
                    { "A1", "seznam_skupin_ro", "skupina A1", "Group A1", true },
                    { "A2", "seznam_skupin_ro", "skupina A2", "Group A2", true },
                    { "AM", "seznam_skupin_ro", "skupina AM", "Group AM", true },
                    { "B", "seznam_skupin_ro", "skupina B", "Group B", true },
                    { "B1", "seznam_skupin_ro", "skupina B1", "Group B1", true },
                    { "BE", "seznam_skupin_ro", "skupina BE", "Group BE", true },
                    { "C", "seznam_skupin_ro", "skupina C", "Group C", true },
                    { "C1", "seznam_skupin_ro", "skupina C1", "Group C1", true },
                    { "C1E", "seznam_skupin_ro", "skupina C1E", "Group C1E", true },
                    { "CE", "seznam_skupin_ro", "skupina CE", "Group CE", true },
                    { "D", "seznam_skupin_ro", "skupina D", "Group D", true },
                    { "D1", "seznam_skupin_ro", "skupina D1", "Group D1", true },
                    { "DE", "seznam_skupin_ro", "skupina DE", "Group DE", true },
                    { "T", "seznam_skupin_ro", "skupina T", "Group T", true },
                    { "skupina_ro_1", "skupina_ro", "skupina 1", "group 1", true },
                    { "skupina_ro_2", "skupina_ro", "skupina 2", "group 2", true },
                    { "stav_posudku_1", "stav_posudku", "platný", "valid", true },
                    { "stav_posudku_2", "stav_posudku", "zneplatněný", "invalidated", true },
                    { "stav_posudku_3", "stav_posudku", "neplatný", "invalid", true },
                    { "vysledek_posudku_ro_1", "vysledek_posudku_ro", "způsobilý", "eligible", true },
                    { "vysledek_posudku_ro_2", "vysledek_posudku_ro", "nezpůsobilý", "unqualified", true },
                    { "vysledek_posudku_ro_3", "vysledek_posudku_ro", "způsobilý s podmínkou", "eligible with conditions", true }
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
