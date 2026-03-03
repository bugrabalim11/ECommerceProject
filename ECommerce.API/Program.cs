using ECommerce.API.Context;
using ECommerce.API.Interfaces;
using ECommerce.API.Repositories;
using ECommerce.API.Services;
using ECommerce.API.ValidationRules;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı köprümüzü (DbContext) ve SQL Server adresimizi sisteme tanıtıyoruz
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. JSON oluştururken sonsuz bir döngü (Cycle) fark edersen onu görmezden gel (IgnoreCycles).
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// 3. Sisteme "CreateProductValidator'ın bulunduğu klasördeki TÜM güvenlik görevlilerini işe al" diyoruz.
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();

builder.Services.AddEndpointsApiExplorer();

// --- ŞİRKETİMİZİN YENİ ÇALIŞANLARI (DEPENDENCY INJECTION) ---

// 1. Genel Depocuyu işe alıyoruz (Joker tip <> olduğu için typeof kullanıyoruz)
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// 2. Kategori Depocusunu işe alıyoruz
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// 3. Kategori Aşçısını (Service) işe alıyoruz
builder.Services.AddScoped<ICategoryService, CategoryService>();

// 4. Ürün Depocusunu işe alıyoruz
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// 5. Ürün Aşçısını (Service) işe alıyoruz
builder.Services.AddScoped<IProductService, ProductService>();

//6. Sipariş Depocusunu işe alıyoruz
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

//7. Sipariş Aşçısını (Service) işe alıyoruz
builder.Services.AddScoped<IOrderService, OrderService>();

// -------------------------------------------------------------

// --- SWAGGER VİTRİN VE KART OKUYUCU AYARLARI ---
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Yaka kartınızı (Token) buraya yapıştırın. Örnek: Bearer eyJhb...",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
}); // <-- DİKKAT: Swagger ayarları BURADA bitti!

// --- JWT (DİJİTAL YAKA KARTI) KİMLİK MAKİNESİ AYARLARI ---
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

var app = builder.Build();

// --- HTTP İSTEK BORU HATTI (PIPELINE) ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// GÜVENLİK GÖREVLİLERİ (SIRALAMA ÇOK ÖNEMLİ!)
app.UseAuthentication(); // 1. ÖNCE KİMLİK KONTROLÜ (Sen kimsin? Kartın var mı?)
app.UseAuthorization();  // 2. SONRA YETKİ KONTROLÜ (İçeri girmeye yetkin var mı?)

app.MapControllers();
app.Run();