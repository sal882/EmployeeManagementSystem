using AutoMapper;
using MVCApplicationTest.DAL.Models;
using MVCApplicationTestPL.ViewModels;

namespace MVCApplicationTestPL.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserViewModel, ApplicationUser>().ReverseMap();
        }
    }

}
