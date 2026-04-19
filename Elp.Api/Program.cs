using Elp.Application.Common.Behaviors;
using Elp.Application.Common.Interfaces;
using Elp.Infrastructure.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddAutoMapper(cfg =>
{
    var licenseKey = builder.Configuration["AutoMapper:LicenseKey"];

    if (!string.IsNullOrEmpty(licenseKey) && licenseKey != "__AUTOMAPPER_LICENSE_KEY__")
    {
        cfg.LicenseKey = licenseKey;
    }
}, typeof(Program));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Elp.Application.Certificates.Commands.CreateCertificate.CreateCertificateCommand).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(Elp.Application.Certificates.Commands.CreateCertificate.CreateCertificateCommand).Assembly);

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Elp.Application.Certificates.Commands.CreateCertificate.CreateCertificateCommand).Assembly);

    // This tells MediatR to run the validation check BEFORE the Handler
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<Elp.Api.Infrastructure.GlobalExceptionHandler>();
builder.Services.AddLocalization();

QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

app.UseExceptionHandler();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var supportedCultures = new[] { "cs", "en" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }