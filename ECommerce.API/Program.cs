using Microsoft.EntityFrameworkCore;
using ECommerce.API.Context;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı köprümüzü (DbContext) ve SQL Server adresimizi sisteme tanıtıyoruz
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// 1. OpenApi yerine Swagger servislerini projeye ekliyoruz
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 2. MapOpenApi yerine Swagger arayüzünü kullanıma açıyoruz
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();