using AutoMapper;
using GonzaShoes.Model.DTOs.Brand;
using GonzaShoes.Model.DTOs.Color;
using GonzaShoes.Model.DTOs.ModelProduct;
using GonzaShoes.Model.DTOs.Order;
using GonzaShoes.Model.DTOs.Product;
using GonzaShoes.Model.DTOs.Size;
using GonzaShoes.Model.DTOs.User;
using GonzaShoes.Model.Entities.Order;
using GonzaShoes.Model.Entities.Product;
using GonzaShoes.Model.Entities.User;

namespace GonzaShoes.Model
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(p => p.Password, opt => opt.Ignore());
            CreateMap<UserDTO, User>()
                .ForMember(p => p.DateCreated, opt => opt.Ignore())
                .ForMember(p => p.CreatedUserId, opt => opt.Ignore())
                .ForMember(p => p.IsActive, opt => opt.Ignore());

            CreateMap<Brand, BrandDTO>();
            CreateMap<BrandDTO, Brand>()
                .ForMember(p => p.DateCreated, opt => opt.Ignore())
                .ForMember(p => p.CreatedUserId, opt => opt.Ignore())
                .ForMember(p => p.IsActive, opt => opt.Ignore());

            CreateMap<Color, ColorDTO>();
            CreateMap<ColorDTO, Color>()
                .ForMember(p => p.DateCreated, opt => opt.Ignore())
                .ForMember(p => p.CreatedUserId, opt => opt.Ignore())
                .ForMember(p => p.IsActive, opt => opt.Ignore());

            CreateMap<ModelProduct, ModelProductDTO>();
            CreateMap<ModelProductDTO, ModelProduct>()
                .ForMember(p => p.DateCreated, opt => opt.Ignore())
                .ForMember(p => p.CreatedUserId, opt => opt.Ignore())
                .ForMember(p => p.IsActive, opt => opt.Ignore());

            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>()
                .ForMember(p => p.DateCreated, opt => opt.Ignore())
                .ForMember(p => p.CreatedUserId, opt => opt.Ignore())
                .ForMember(p => p.IsActive, opt => opt.Ignore());

            CreateMap<Size, SizeDTO>();
            CreateMap<SizeDTO, Size>()
                .ForMember(p => p.DateCreated, opt => opt.Ignore())
                .ForMember(p => p.CreatedUserId, opt => opt.Ignore())
                .ForMember(p => p.IsActive, opt => opt.Ignore());

            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>()
                .ForMember(p => p.DateCreated, opt => opt.Ignore())
                .ForMember(p => p.CreatedUserId, opt => opt.Ignore())
                .ForMember(p => p.IsActive, opt => opt.Ignore());

            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<OrderItemDTO, OrderItem>()
                .ForMember(p => p.DateCreated, opt => opt.Ignore())
                .ForMember(p => p.CreatedUserId, opt => opt.Ignore())
                .ForMember(p => p.IsActive, opt => opt.Ignore());

            CreateMap<OrderPayment, OrderPaymentDTO>();
            CreateMap<OrderPaymentDTO, OrderPayment>()
                .ForMember(p => p.DateCreated, opt => opt.Ignore())
                .ForMember(p => p.CreatedUserId, opt => opt.Ignore())
                .ForMember(p => p.IsActive, opt => opt.Ignore());
        }
    }
}
