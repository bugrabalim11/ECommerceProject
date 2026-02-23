// Entity Framework ve Context sınıflarımızı kullanabilmek için gerekli kütüphaneler
using Microsoft.EntityFrameworkCore;
using ECommerce.API.Context; // Context klasörünün yolunu kendi projene göre ayarla

var builder = WebApplication.CreateBuilder(args);

// Veritabanı köprümüzü (DbContext) ve SQL Server adresimizi sisteme tanıtıyoruz
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
