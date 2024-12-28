//using API.Context;

using API.Context;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<Employee>(options => { })
    .AddRoles<EmployeeRole>()
    .AddEntityFrameworkStores<ApiDbContext>()
    .AddDefaultTokenProviders()
    .AddApiEndpoints();

builder.Services.AddIdentityCore<Customer>(options => { })
    .AddEntityFrameworkStores<ApiDbContext>()
    .AddDefaultTokenProviders()
    .AddApiEndpoints();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddDataProtection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<Employee>();
//app.MapIdentityApi<Customer>();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();