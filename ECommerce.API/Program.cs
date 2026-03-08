using ECommerce.API.Context;
using ECommerce.API.Interfaces;
using ECommerce.API.Repositories;
using ECommerce.API.Services;
using ECommerce.API.ValidationRules;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AutoMapper;
using ECommerce.API.Mappings;


var builder = WebApplication.CreateBuilder(args);

// --- 1. TEMEL AYARLAR ---
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
builder.Services.AddEndpointsApiExplorer();

// --- 2. GÜVENLİK VE KİMLİK (JWT) AYARLARI ---
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


// Sisteme "Gidip MappingProfile sınıfının olduğu yeri bul ve çalıştır" diyoruz.
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// --- 3. DEPENDENCY INJECTION (İŞÇİ KAYITLARI) ---
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// --- 4. SWAGGER (VİTRİN) AYARLARI ---
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        // İŞTE BURAYI DEĞİŞTİRİYORUZ! Artık kimse "Bearer" yazmayacak.
        Description = "Lütfen SADECE Token'ı (eyJ...) buraya yapıştırın. Başına 'Bearer' YAZMAYIN, sistem otomatik ekleyecektir.",
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
});

var app = builder.Build();

// --- 5. HTTP İSTEK BORU HATTI (PIPELINE) ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// GÜVENLİK KORUMALARI (Sıralama kusursuz!)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();