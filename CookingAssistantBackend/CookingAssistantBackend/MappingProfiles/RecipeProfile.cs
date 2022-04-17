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
                .ReverseMap()
                .ForPath(s => s.User.UserId, opt => opt.MapFrom(src => src.UserId));


            CreateMap<RecipeStep, RecipeStepDto>().ReverseMap();

            CreateMap<Tag, TagDto>().ReverseMap();

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.WrittenById, opt => opt.MapFrom(src => src.WrittenBy.UserId))
                .ReverseMap()
                .ForPath(s => s.WrittenBy.UserId, opt => opt.MapFrom(src => src.WrittenById));

            CreateMap<Like, LikeDto>()
                .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.Comment.CommentId))
                .ForMember(dest => dest.LikedById, opt => opt.MapFrom(src => src.LikedBy.UserId))
                .ReverseMap()
                .ForPath(s => s.Comment.CommentId, opt => opt.MapFrom(src => src.CommentId))
                .ForPath(s => s.LikedBy.UserId, opt => opt.MapFrom(src => src.LikedById));

        }
    }
}
