using Arbiter.API.Data;
using Arbiter.API.Hubs;
using Arbiter.API.Services;
using Arbiter.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System;
using Arbiter.API.Middlewares;
using Serilog;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Arbiter.API.Auth;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? throw new Exception("ASPNETCORE_ENVIRONMENT IS EMPTY");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{environment}.json", false, true)
    .AddEnvironmentVariables()
    .Build();

var connStringToLogBlob = configuration["BlobStorageConnectionString"];

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.AzureBlobStorage(
        connectionString: connStringToLogBlob,
        storageContainerName: configuration["BlobStorage-ContainerName"],
        storageFileName: $"{DateTime.UtcNow:yyyyMMdd}.txt")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("VolleyballDB"));
});

builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddTransient<IMatchService, MatchService>();
builder.Services.AddTransient<IMatchReportService, MatchReportService>();
builder.Services.AddTransient<ITextAnalyzerService, KeyPhraseAnalyzerService>();
builder.Services.AddHostedService<ServiceBusBackgroundService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "ApiKey";
    options.DefaultChallengeScheme = "ApiKey";
})
.AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<MatchReportTextAnalyzerHub>("/hub/match-report-text-anaylzer");

app.Run();
