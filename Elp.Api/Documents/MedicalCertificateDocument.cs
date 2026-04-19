using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Elp.Api.Models;

namespace Elp.Api.Documents;

public class MedicalCertificateDocument : IDocument
{
    private readonly PosudekRoDetailDto _model;

    public MedicalCertificateDocument(PosudekRoDetailDto model)
    {
        _model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily(Fonts.Arial));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
    }

    void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text("LÉKAŘSKÝ POSUDEK").FontSize(24).SemiBold().FontColor(Colors.Blue.Darken2);
                column.Item().Text("o zdravotní způsobilosti k řízení motorových vozidel").FontSize(14).FontColor(Colors.Grey.Darken2);
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(1, Unit.Centimetre).Column(column =>
        {
            column.Spacing(10);

            // Driver Section
            column.Item().Background(Colors.Grey.Lighten4).Padding(10).Column(c =>
            {
                c.Item().Text("Informace o řidiči").SemiBold().FontSize(14);
                c.Item().Text($"Identifikátor řidiče (RID): {_model.Rid}");
            });

            // Doctor Section
            column.Item().Background(Colors.Grey.Lighten4).Padding(10).Column(c =>
            {
                c.Item().Text("Informace o lékaři").SemiBold().FontSize(14);
                c.Item().Text($"Identifikátor lékaře (KrzpId): {_model.KrzpId}");
            });

            // Result Section
            column.Item().PaddingTop(15).Text("Závěr lékařské prohlídky").SemiBold().FontSize(16);

            // Highlight the status text
            column.Item().Text(text =>
            {
                text.Span("Stav posudku: ");
                text.Span(_model.StavPosudku?.Kod ?? "Neznámý").SemiBold().FontColor(Colors.Blue.Medium);
            });

            column.Item().Text($"Datum vystavení: {_model.DatumVystaveni:dd.MM.yyyy}");
        });
    }

    void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(x =>
        {
            x.Span("Strana ");
            x.CurrentPageNumber();
            x.Span(" z ");
            x.TotalPages();
        });
    }
}