using Microsoft.EntityFrameworkCore;
using Elp.Domain.Entities;
using Elp.Application.Common.Interfaces;

namespace Elp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<DriverFitnessCertificate> DriverFitnessCertificates => Set<DriverFitnessCertificate>();
    public DbSet<Codebook> Codebooks => Set<Codebook>();
    public DbSet<CodebookItem> CodebookItems => Set<CodebookItem>();
    public DbSet<FitnessGroup> FitnessGroups => Set<FitnessGroup>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<DriverFitnessCertificate>(entity =>
        {
            // Tell EF Core to use this for concurrency tracking
            entity.Property(e => e.RowVersion)
                  .IsRowVersion();
        });

        // Define Primary Keys explicitly
        builder.Entity<Codebook>().HasKey(c => c.Code);

        // Composite key for Codebook Items (CodebookCode + ItemCode)
        builder.Entity<CodebookItem>().HasKey(ci => new { ci.CodebookCode, ci.Code });

        // Primary key for FitnessGroup
        builder.Entity<FitnessGroup>().HasKey(fg => fg.Id);


        // TODO

        builder.Entity<Codebook>().HasData(
            new Codebook { Code = "akce_ro", NameCz = "akce ro", NameEn = "event" },
            new Codebook { Code = "druh_prohlidky_ro", NameCz = "druh prohlidky ro", NameEn = "type of inspection" },
            new Codebook { Code = "skupina_ro", NameCz = "skupina ro", NameEn = "group" },
            new Codebook { Code = "vysledek_posudku_ro", NameCz = "vysledek posudku ro", NameEn = "assessment result" },
            new Codebook { Code = "druh_posudku_ro", NameCz = "druh posudku ro", NameEn = "assessment type" },
            new Codebook { Code = "seznam_skupin_ro", NameCz = "seznam skupin", NameEn = "vehicle type" },
            new Codebook { Code = "seznam_harmonizovane_kody_ro", NameCz = "harmonizovane kody", NameEn = "harmonized codes" },
            new Codebook { Code = "seznam_narodni_kody_ro", NameCz = "harmonizovane kody", NameEn = "country codes" },
            new Codebook { Code = "stav_posudku", NameCz = "stav posudku", NameEn = "assessment status" }
        );

        builder.Entity<CodebookItem>().HasData(

            new CodebookItem { CodebookCode = "akce_ro", Code = "akce_ro_1", DescriptionCz = "vytvoření", DescriptionEn = "creation", IsActive = true },
            new CodebookItem { CodebookCode = "akce_ro", Code = "akce_ro_2", DescriptionCz = "aktualizace", DescriptionEn = "update", IsActive = true },
            new CodebookItem { CodebookCode = "akce_ro", Code = "akce_ro_3", DescriptionCz = "zneplatnění", DescriptionEn = "invalidation", IsActive = true },

            new CodebookItem { CodebookCode = "druh_prohlidky_ro", Code = "druh_prohlidky_ro3", DescriptionCz = "mimořádná", DescriptionEn = "extraordinary", IsActive = true },
            new CodebookItem { CodebookCode = "druh_prohlidky_ro", Code = "druh_prohlidky_ro2", DescriptionCz = "pravidelná", DescriptionEn = "regular", IsActive = true },
            new CodebookItem { CodebookCode = "druh_prohlidky_ro", Code = "druh_prohlidky_ro1", DescriptionCz = "vstupní", DescriptionEn = "input", IsActive = true },


            new CodebookItem { CodebookCode = "skupina_ro", Code = "skupina_ro_1", DescriptionCz = "skupina 1", DescriptionEn = "group 1", IsActive = true },
            new CodebookItem { CodebookCode = "skupina_ro", Code = "skupina_ro_2", DescriptionCz = "skupina 2", DescriptionEn = "group 2", IsActive = true },


            new CodebookItem { CodebookCode = "vysledek_posudku_ro", Code = "vysledek_posudku_ro_1", DescriptionCz = "způsobilý", DescriptionEn = "eligible", IsActive = true },
            new CodebookItem { CodebookCode = "vysledek_posudku_ro", Code = "vysledek_posudku_ro_2", DescriptionCz = "nezpůsobilý", DescriptionEn = "unqualified", IsActive = true },
            new CodebookItem { CodebookCode = "vysledek_posudku_ro", Code = "vysledek_posudku_ro_3", DescriptionCz = "způsobilý s podmínkou", DescriptionEn = "eligible with conditions", IsActive = true },


            new CodebookItem { CodebookCode = "druh_posudku_ro", Code = "druh_posudku_ro_2", DescriptionCz = "rozšíření řidičského oprávnění", DescriptionEn = "Driver's license extension", IsActive = true },
            new CodebookItem { CodebookCode = "druh_posudku_ro", Code = "druh_posudku_ro_1", DescriptionCz = "prvořidič", DescriptionEn = "lead driver", IsActive = true },
            new CodebookItem { CodebookCode = "druh_posudku_ro", Code = "druh_posudku_ro_4", DescriptionCz = "senioři", DescriptionEn = "seniors", IsActive = true },
            new CodebookItem { CodebookCode = "druh_posudku_ro", Code = "druh_posudku_ro_3", DescriptionCz = "prodloužení řidičského oprávnění", DescriptionEn = "renewal of a driver's license", IsActive = true },
            new CodebookItem { CodebookCode = "druh_posudku_ro", Code = "druh_posudku_ro_5", DescriptionCz = "přezkoumání způsobilosti", DescriptionEn = "eligibility review", IsActive = true },

            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina B", Code = "B", DescriptionEn = "Group B", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina A", Code = "A", DescriptionEn = "Group A", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina D", Code = "D", DescriptionEn = "Group D", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina BE", Code = "BE", DescriptionEn = "Group BE", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina C", Code = "C", DescriptionEn = "Group C", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina DE", Code = "DE", DescriptionEn = "Group DE", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina CE", Code = "CE", DescriptionEn = "Group CE", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina D1", Code = "D1", DescriptionEn = "Group D1", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina C1", Code = "C1", DescriptionEn = "Group C1", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina C1E", Code = "C1E", DescriptionEn = "Group C1E", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina AM", Code = "AM", DescriptionEn = "Group AM", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina T", Code = "T", DescriptionEn = "Group T", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina A2", Code = "A2", DescriptionEn = "Group A2", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina B1", Code = "B1", DescriptionEn = "Group B1", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_skupin_ro", DescriptionCz = "skupina A1", Code = "A1", DDescriptionEn = "Group A1", IsActive = true },

new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "43.01", DescriptionCz = "43.01 Výška sedadla řidiče umožňující normální výhled a v normální vzdálenosti od  volantu a pedálů", DescriptionEn = "43.01 Driver's seat height allowing normal view and at a normal distance from the steering wheel and pedals", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "10.02", DescriptionCz = "10.02 Automatická převodovka", DescriptionEn = "10.02 Automatic transmission", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "10.04", DescriptionCz = "10.04 Přizpůsobené ústrojí ovládání převodovky", DescriptionEn = "10.04 Adapted transmission control mechanism", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "20.07", DescriptionCz = "20.07 Ovládání brzdy s použitím maximální síly ... N [například: 20.07(300 N)]", DescriptionEn = "20.07 Brake operation using a maximum force of ... N [for example: 20.07(300 N)]", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "25.09", DescriptionCz = "25.09 Opatření proti zablokování nebo aktivaci akcelerátoru", DescriptionEn = "25.09 Measures against blocking or activating the accelerator", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "35.05", DescriptionCz = "35.05 Ovladače ovladatelné bez puštění zařízení pro řízení vozidla a mechanismy akcelerátoru a brzd", DescriptionEn = "35.05 Controls operable without releasing the steering device and accelerator and brake mechanisms", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "20.03", DescriptionCz = "20.03 Brzdový pedál upravený na levou nohu", DescriptionEn = "20.03 Brake pedal adapted for the left foot", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "31.04", DescriptionCz = "31.04 Zvýšená podlaha", DescriptionEn = "31.04 Raised floor", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "25.01", DescriptionCz = "25.01 Přizpůsobený pedál akcelerátoru", DescriptionEn = "25.01 Adapted accelerator pedal", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "44.02", DescriptionCz = "44.02 Přizpůsobená brzda na předním kole", DescriptionEn = "44.02 Adapted front wheel brake", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "42.05", DescriptionCz = "42.05 Zařízení k eliminaci mrtvého úhlu", DescriptionEn = "42.05 Device to eliminate blind spots", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "42.01", DescriptionCz = "42.01 Přizpůsobené zařízení pro výhled dozadu", DescriptionEn = "42.01 Adapted rear-view device", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "25.05", DescriptionCz = "25.05 Ovládání akcelerátoru kolenem", DescriptionEn = "25.05 Knee-operated accelerator", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "44.10", DescriptionCz = "44.10 Maximální ovládací síla brzdy zadního kola... N [například 44.10 (240 N)]", DescriptionEn = "44.10 Maximum operating force of the rear wheel brake... N [for example 44.10 (240 N)]", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "63", DescriptionCz = "63. Řízení vozidla bez cestujících", DescriptionEn = "63. Driving a vehicle without passengers", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "64", DescriptionCz = "64. Jízda rychlostí nepřesahující ... km/h", DescriptionEn = "64. Driving at a speed not exceeding ... km/h", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "61", DescriptionCz = "61. Omezení jízdy podle denní doby (například: jedna hodina po východu slunce a jedna hodina před západem slunce)", DescriptionEn = "61. Driving restriction by time of day (for example: one hour after sunrise and one hour before sunset)", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "62", DescriptionCz = "62. Omezení jízdy v okruhu ... km od místa bydliště řidiče nebo pouze ve městě/regionu", DescriptionEn = "62. Driving restriction within a radius of ... km from the driver's place of residence or only within a city/region", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "40.05", DescriptionCz = "40.05 Přizpůsobený volant (větší nebo silnější volant, zmenšený průměr volantu, apod.)", DescriptionEn = "40.05 Adapted steering wheel (larger or thicker steering wheel, reduced steering wheel diameter, etc.)", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_harmonizovane_kody_ro", Code = "67", DescriptionCz = "67. Zákaz jízdy na dálnici", DescriptionEn = "67. Driving on motorways prohibited", IsActive = true },

            new CodebookItem { CodebookCode = "seznam_narodni_kody_ro", Code = "185", DescriptionCz = "185. Pouze pro řízení motorových vozidel stanovených v § 83 odst. 5 zákona č. 361/2000 Sb. do doby dosažení věku u skupiny vozidel C 21 let, u skupiny vozidel D 24 let", DescriptionEn = "185. Only for driving motor vehicles specified in Section 83, Paragraph 5 of Act No. 361/2000 Coll. until reaching the age of 21 for vehicle category C, and 24 for vehicle category D", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_narodni_kody_ro", Code = "111a", DescriptionCz = "111. Nelze vykonávat činnost: a) řidiče, který řídí motorové vozidlo v pracovněprávním vztahu a u něhož je řízení motorového vozidla druhem práce sjednaným v pracovní smlouvě", DescriptionEn = "111. Cannot perform the activity of: a) a driver who drives a motor vehicle in an employment relationship and for whom driving a motor vehicle is the type of work agreed upon in the employment contract", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_narodni_kody_ro", Code = "111b", DescriptionCz = "111. Nelze vykonávat činnost: b) řidiče, u kterého je řízení motorového vozidla předmětem samostatné výdělečné činnosti prováděné podle jiného právního předpisu", DescriptionEn = "111. Cannot perform the activity of: b) a driver for whom driving a motor vehicle is the subject of self-employment carried out under another legal regulation", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_narodni_kody_ro", Code = "175", DescriptionCz = "175. Omezení řidičského oprávnění skupiny vozidel D pouze k řízení vozidla městské hromadné dopravy osob", DescriptionEn = "175. Restriction of the driving license for vehicle category D only to driving a public transport vehicle", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_narodni_kody_ro", Code = "111c", DescriptionCz = "111. Nelze vykonávat činnost: c) učitele výcviku v řízení motorových vozidel podle jiného právního předpisu", DescriptionEn = "111. Cannot perform the activity of: c) a driving school instructor under another legal regulation", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_narodni_kody_ro", Code = "160", DescriptionCz = "160. Výjimka z věku u řidičského oprávnění skupiny vozidel A1, A2 nebo A osoby, která je držitelem licence motoristického sportovce, udělená pouze pro jízdu při sportovní soutěži", DescriptionEn = "160. Age exemption for a driving license of vehicle category A1, A2 or A for a person holding a motorsport athlete license, granted only for driving during a sports competition", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_narodni_kody_ro", Code = "172", DescriptionCz = "172. Omezení řidičského oprávnění skupiny vozidel A pouze k řízení motorového vozíku pro invalidy", DescriptionEn = "172. Restriction of the driving license for vehicle category A only to driving a motorized wheelchair for the disabled", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_narodni_kody_ro", Code = "115", DescriptionCz = "115. Posilovač spojky", DescriptionEn = "115. Power clutch", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_narodni_kody_ro", Code = "105", DescriptionCz = "105. Jiná zdravotní omezení, která nejsou uvedena v části I této přílohy", DescriptionEn = "105. Other health restrictions not listed in Part I of this Annex", IsActive = true },
            new CodebookItem { CodebookCode = "seznam_narodni_kody_ro", Code = "199", DescriptionCz = "199. Zkušební doba podmíněného upuštění od výkon", DescriptionEn = "199. Probationary period of conditional waiver of execution", IsActive = true },


            new CodebookItem { CodebookCode = "stav_posudku", Code = "stav_posudku_1", DescriptionCz = "platný", DescriptionEn = "valid", IsActive = true },
            new CodebookItem { CodebookCode = "stav_posudku", Code = "stav_posudku_2", DescriptionCz = "zneplatněný", DescriptionEn = "invalidated", IsActive = true },
            new CodebookItem { CodebookCode = "stav_posudku", Code = "stav_posudku_3", DescriptionCz = "neplatný", DescriptionEn = "invalid", IsActive = true }

        );

        // builder.Entity<Codebook>().HasData(
        //     new Codebook { Code = "StavPosudku", NameCz = "Stavy lékařských posudků", NameEn = "Medical Certificate Statuses" },
        //     new Codebook { Code = "TypOpravneni", NameCz = "Typy řidičských oprávnění", NameEn = "Driving License Types" }
        // );

        // builder.Entity<CodebookItem>().HasData(
        //     // --- Statuses ---
        //     new CodebookItem { CodebookCode = "StavPosudku", Code = "VYDANO", DescriptionCz = "Posudek byl vydán", DescriptionEn = "Certificate Issued", IsActive = true },
        //     new CodebookItem { CodebookCode = "StavPosudku", Code = "ZAMITNUTO", DescriptionCz = "Posudek byl zamítnut", DescriptionEn = "Certificate Rejected", IsActive = true },
        //     new CodebookItem { CodebookCode = "StavPosudku", Code = "ZNEPLATNENO", DescriptionCz = "Posudek byl zneplatněn", DescriptionEn = "Certificate Invalidated", IsActive = true },

        //     // --- Driving License Types ---
        //     new CodebookItem { CodebookCode = "TypOpravneni", Code = "A", DescriptionCz = "Motocykly", DescriptionEn = "Motorcycles", IsActive = true },
        //     new CodebookItem { CodebookCode = "TypOpravneni", Code = "B", DescriptionCz = "Osobní automobily", DescriptionEn = "Passenger cars", IsActive = true },
        //     new CodebookItem { CodebookCode = "TypOpravneni", Code = "C", DescriptionCz = "Nákladní automobily", DescriptionEn = "Trucks", IsActive = true },
        //     new CodebookItem { CodebookCode = "TypOpravneni", Code = "D", DescriptionCz = "Autobusy", DescriptionEn = "Buses", IsActive = true },
        //     new CodebookItem { CodebookCode = "TypOpravneni", Code = "T", DescriptionCz = "Traktory", DescriptionEn = "Tractors", IsActive = true }
        // );

        var doctorSmithId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var doctorNovakId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        builder.Entity<DriverFitnessCertificate>().HasData(
            new
            {
                Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                PersonalId = "ABC-123-456",
                MedicalProfessionalId = doctorSmithId,
                IssueDate = new DateTime(2025, 1, 15, 8, 0, 0, DateTimeKind.Utc),
                StatusCode = "VYDANO",
            },
            new
            {
                Id = Guid.Parse("b2222222-2222-2222-2222-222222222222"),
                PersonalId = "XYZ-987-654",
                MedicalProfessionalId = doctorSmithId,
                IssueDate = new DateTime(2026, 2, 10, 9, 30, 0, DateTimeKind.Utc),
                StatusCode = "ZAMITNUTO",
            },
            new
            {
                Id = Guid.Parse("c3333333-3333-3333-3333-333333333333"),
                PersonalId = "ABC-123-456",
                MedicalProfessionalId = doctorNovakId,
                IssueDate = new DateTime(2020, 5, 20, 14, 15, 0, DateTimeKind.Utc),
                StatusCode = "ZNEPLATNENO",
            },
            new
            {
                Id = Guid.Parse("d4444444-4444-4444-4444-444444444444"),
                PersonalId = "LMN-456-789",
                MedicalProfessionalId = doctorNovakId,
                IssueDate = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
                StatusCode = "VYDANO",
            }
        );
    }
}