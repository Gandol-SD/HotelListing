using HotelListing.API.Configurations;
using HotelListing.API.Data;
using HotelListing.API.Data.Repositories;
using HotelListing.API.Data.RepositoryInterfaces;
using HotelListing.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var conString = builder.Configuration.GetConnectionString("HotelListingDBConnectionString");
builder.Services.AddDbContext<ApiDBX>(options =>
{
    options.UseSqlServer(conString);
});

builder.Services.AddIdentityCore<HotelUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApiDBX>();


builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IHotelsRepository, HotelsRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyHeader()
                                        .AllowAnyOrigin()
                                        .AllowAnyMethod());
});

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .WriteTo.File(path: "./logs/log-.txt",rollingInterval:Serilog.RollingInterval.Hour)
    /*.ReadFrom.Configuration(context.Configuration)*/);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
