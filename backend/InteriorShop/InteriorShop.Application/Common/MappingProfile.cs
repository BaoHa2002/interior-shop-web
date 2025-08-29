using AutoMapper;
using InteriorShop.Application.DTOs.Auth;
using InteriorShop.Application.DTOs.Banners;
using InteriorShop.Application.DTOs.Category;
using InteriorShop.Application.DTOs.Content;
using InteriorShop.Application.DTOs.Option;
using InteriorShop.Application.DTOs.Orders;
using InteriorShop.Application.DTOs.Products;
using InteriorShop.Application.DTOs.Settings;
using InteriorShop.Application.DTOs.Users;
using InteriorShop.Application.Requests.Auth;
using InteriorShop.Application.Requests.Banners;
using InteriorShop.Application.Requests.Categories;
using InteriorShop.Application.Requests.Content;
using InteriorShop.Application.Requests.Options;
using InteriorShop.Application.Requests.Orders;
using InteriorShop.Application.Requests.Products;
using InteriorShop.Application.Requests.Settings;
using InteriorShop.Application.Requests.Users;
using InteriorShop.Domain.Entities;

namespace InteriorShop.Application.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ===== USER / AUTH =====
            CreateMap<UserUpdateRequest, UserDto>().ReverseMap();
            CreateMap<RegisterRequest, UserDto>();

            // ===== CATEGORY =====
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CategoryCreateRequest, Category>();
            CreateMap<CategoryUpdateRequest, Category>();

            // ===== PRODUCT =====
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductVariant, ProductVariantDto>().ReverseMap();
            CreateMap<Product, CartProductDto>().ReverseMap();

            CreateMap<ProductCreateRequest, Product>();
            CreateMap<ProductUpdateRequest, Product>();
            CreateMap<ProductQueryRequest, Product>();

            // ===== OPTION =====
            CreateMap<OptionGroup, OptionGroupDto>().ReverseMap();
            CreateMap<OptionValue, OptionValueDto>().ReverseMap();

            CreateMap<OptionGroupCreateRequest, OptionGroup>();
            CreateMap<OptionGroupUpdateRequest, OptionGroup>();
            CreateMap<OptionValueCreateRequest, OptionValue>();
            CreateMap<OptionValueUpdateRequest, OptionValue>();

            // ===== ORDER =====
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<OrderCreateRequest, Order>();
            CreateMap<OrderItemRequest, OrderItem>();
            CreateMap<OrderItemCreateRequest, OrderItem>();
            CreateMap<UpdateOrderStatusRequest, Order>();

            // ===== BANNERS =====
            CreateMap<BannerSlide, BannerDto>().ReverseMap();
            CreateMap<BannerCreateRequest, BannerSlide>();
            CreateMap<BannerUpdateRequest, BannerSlide>();

            CreateMap<CampaignBanner, CampaignBannerDto>().ReverseMap();
            CreateMap<CampaignBannerCreateRequest, CampaignBanner>();
            CreateMap<CampaignBannerUpdateRequest, CampaignBanner>();

            CreateMap<PromoTile, PromoTileDto>().ReverseMap();
            CreateMap<PromoTileCreateRequest, PromoTile>();
            CreateMap<PromoTileUpdateRequest, PromoTile>();

            // ===== CONTENT =====
            CreateMap<BlogPost, BlogPostDto>().ReverseMap();
            CreateMap<BlogPostCreateRequest, BlogPost>();
            CreateMap<BlogPostUpdateRequest, BlogPost>();

            // ===== SETTINGS =====
            CreateMap<SiteSetting, SiteSettingDto>().ReverseMap();
            CreateMap<UpdateSiteSettingRequest, SiteSetting>();
        }
    }
}