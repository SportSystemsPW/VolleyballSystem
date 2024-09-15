using Arbiter.API.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<AzureSpeechServiceOptions>()
    .Bind(builder.Configuration.GetSection("azureSpeechService"))
    .ValidateDataAnnotations()
    .ValidateOnStart();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy
            .WithOrigins("http://localhost:5299")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
