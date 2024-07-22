using AutoMapper;
using LemonLime.DTOs.Auth;
using LemonLime.DTOs.Comment;
using LemonLime.DTOs.Image;
using LemonLime.DTOs.NutritionInfo;
using LemonLime.DTOs.Rating;
using LemonLime.DTOs.Recipe;
using LemonLime.DTOs.Role;
using LemonLime.DTOs.Tag;
using LemonLime.DTOs.User;
using LemonLime.Models;

namespace LemonLime.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Role, RoleRequest>().ReverseMap();
            CreateMap<Role, RoleResponse>().ReverseMap();

            CreateMap<Tag, TagResponse>().ReverseMap();
            CreateMap<TagRequest, Tag>().ReverseMap();

            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<UserRequest, User>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
            CreateMap<User, UserRequest>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
            CreateMap<User, UserProfileResponse>()
                .ForMember(dest => dest.RecipeCount, opt => opt.Ignore()).ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<User, UserDetailsResponse>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name)).ReverseMap();

            CreateMap<Recipe, RecipeHomeResponse>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault().Url))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Ratings.Any() ? src.Ratings.Average(rat => rat.Value) : 0))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count));

            CreateMap<User, UserHomeResponse>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
                .ForMember(dest => dest.RecipeCount, opt => opt.MapFrom(src => src.Recipes.Count))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Recipes.Any() ? src.Recipes.Average(r => r.Ratings.Any() ? r.Ratings.Average(rat => rat.Value) : 0) : 0));

            CreateMap<Tag, TagHomeResponse>()
                .ForMember(dest => dest.RecipeCount, opt => opt.MapFrom(src => src.RecipeTags.Count));

            CreateMap<Image, ImageResponse>().ReverseMap();
            CreateMap<Tag, TagResponse>().ReverseMap();
            CreateMap<NutritionInfo, NutritionInfoResponse>();
            CreateMap<NutritionInfo, NutritionInfoRequest>().ReverseMap();
            CreateMap<Comment, CommentResponse>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            CreateMap<Recipe, RecipeResponse>()
                .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src => src.CreatedByUser))
                .ForMember(dest => dest.NutritionInfo, opt => opt.MapFrom(src => src.NutritionInfo))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.RecipeTags.Select(rt => rt.Tag)))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Ratings.Any() ? src.Ratings.Average(r => r.Value) : 0));

            CreateMap<RecipeRequest, Recipe>()
                .ForMember(dest => dest.NutritionInfo, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.RecipeTags, opt => opt.Ignore());

            CreateMap<Recipe, RecipeEditRequest>()
                .ForMember(dest => dest.NutritionInfo, opt => opt.MapFrom(src => src.NutritionInfo))
                .ReverseMap();

            CreateMap<Recipe, RecipeRequest>()
                .ForMember(dest => dest.NutritionInfo, opt => opt.MapFrom(src => src.NutritionInfo))
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.RecipeTags.Select(rt => rt.TagId).ToList()));

            CreateMap<CommentRequest, Comment>().ReverseMap();
            CreateMap<CommentRequest, Rating>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Rating)).ReverseMap();

        }
    }
}
