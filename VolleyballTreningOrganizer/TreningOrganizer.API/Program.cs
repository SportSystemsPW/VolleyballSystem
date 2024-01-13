using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using TreningOrganizer.API.IRepositories;
using TreningOrganizer.API.IServices;
using TreningOrganizer.API.Repositories;
using TreningOrganizer.API.Services;
using Volleyball.Infrastructure.Database.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<VolleyballContext>(options =>
{
    options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=volleyball;Integrated Security=True;Connect Timeout=60;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
});

builder.Services.AddScoped<IMessageTemplateRepository, MessageTemplateRepository>();
builder.Services.AddScoped<ITrainingGroupRepository, TrainingGroupRepository>();
builder.Services.AddScoped<ITrainingParticipantRepository, TrainingParticipantRepository>();
builder.Services.AddScoped<ITrainingRepository, TrainingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IMessageTemplateService, MessageTemplateService>();
builder.Services.AddScoped<ITrainingGroupService, TrainingGroupService>();
builder.Services.AddScoped<ITrainingParticipantService, TrainingParticipantService>();
builder.Services.AddScoped<ITrainingService, TrainingService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidIssuer = "*",
        ValidateAudience = false,
        ValidAudience = "*",
        ClockSkew = TokenValidationParameters.DefaultClockSkew,
        RoleClaimType = "authorities",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

});
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var app = builder.Build();

// Configure the HTTP request pipeline.
#if !DEBUG
app.UseHttpsRedirection();
#endif

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
