using AutoMapper;
using GonzaShoes.Model.DTOs.User;
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
                .ForMember(p => p.CreatedUserId, opt => opt.Ignore());
        }
    }
}
