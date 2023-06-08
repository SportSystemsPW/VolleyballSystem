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

builder.Services.AddScoped<IMessageTemplateService, MessageTemplateService>();
builder.Services.AddScoped<IMessageTemplateRepository, MessageTemplateRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
