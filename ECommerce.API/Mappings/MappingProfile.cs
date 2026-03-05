using AutoMapper;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.UserDtos;
using ECommerce.API.DTOs.CategoryDtos;
using ECommerce.API.DTOs.ProductDtos;

namespace ECommerce.API.Mappings
{
    // Profile sınıfından miras alıyoruz ki AutoMapper buranın bir harita olduğunu anlasın
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // --- Kullanıcı (User) HARİTALARI ---
            // ReverseMap() sayesinde bu çeviri iki yönlü de çalışır:
            // İster User'ı CreateUserDto'ya çevir, ister CreateUserDto'yu User'a çevir!
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<User, ResultUserDto>().ReverseMap();

            // --- KATEGORİ (CATEGORY) HARİTALARI ---
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, ResultCategoryDto>().ReverseMap();

            // --- ÜRÜN (PRODUCT) HARİTALARI ---

            // Ürün ekleme (POST) ve güncelleme (PUT) işlemleri için ham nesne ile DTO arası köprü
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();

            // Ürün listeleme (GET) haritası:
            // Burada 'Flattening' (Düzleştirme) yapıyoruz.
            // Amacımız: Ürün içindeki karmaşık 'Category' nesnesine git, 
            // o nesnenin içindeki 'CategoryName' metnini al ve DTO'daki 'CategoryName' alanına yapıştır.
            CreateMap<Product, ResultProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ReverseMap();
        }
    }
}
