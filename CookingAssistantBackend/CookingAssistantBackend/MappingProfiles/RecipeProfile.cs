using AutoMapper;
using CookingAssistantBackend.Models;
using CookingAssistantBackend.Models.DTOs;

namespace CookingAssistantBackend.MappingProfiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipeDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ReverseMap()
                .ForMember(s => s.RecipeId, opt => opt.Ignore());

            CreateMap<RecipeStep, RecipeStepDto>().ReverseMap();

            CreateMap<Tag, TagDto>().ReverseMap();

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.WrittenById, opt => opt.MapFrom(src => src.WrittenBy.UserId))
                .ForMember(dest => dest.WrittenByName, opt => opt.MapFrom(src => src.WrittenBy.Name))
                .ReverseMap();

            CreateMap<Like, LikeDto>()
                .ReverseMap();
        }
    }
}
