using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TreningOrganizer.API.IRepositories;
using TreningOrganizer.API.IServices;
using TreningOrganizer.API.Repositories;
using TreningOrganizer.API.Services;
using Volleyball.Infrastructure.Database.Models;

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

builder.Services.AddScoped<IMessageTemplateService, MessageTemplateService>();
builder.Services.AddScoped<ITrainingGroupService, TrainingGroupService>();
builder.Services.AddScoped<ITrainingParticipantService, TrainingParticipantService>();
builder.Services.AddScoped<ITrainingService, TrainingService>();

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
