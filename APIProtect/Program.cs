using FluentValidation.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Weather.Core.DTOs;
using Weather.Core.Interfaces;
using Weather.Core.Services;
using Weather.Core.Utility;
using APIProtect.Extensions;
using Weather.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddRegisterServices(configuration);
builder.Services.AddControllersExtension();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(WeatherProfile));
builder.Services.AddDbContextAndConfigurations(builder.Environment, builder.Configuration);
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();
await WeatherDbInitializer.Seed(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
