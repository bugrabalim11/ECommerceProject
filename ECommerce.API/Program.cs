using Microsoft.EntityFrameworkCore;
using ECommerce.API.Context;
using FluentValidation;
using ECommerce.API.ValidationRules;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı köprümüzü (DbContext) ve SQL Server adresimizi sisteme tanıtıyoruz
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// JSON oluştururken sonsuz bir döngü (Cycle) fark edersen onu görmezden gel (IgnoreCycles).
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// Sisteme "CreateProductValidator'ın bulunduğu klasördeki TÜM güvenlik görevlilerini işe al" diyoruz.
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
// !!!!! Kodu, projeyi tarayıp o klasördeki bütün güvenlik görevlilerini otomatik olarak işe aldı !!!!!

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