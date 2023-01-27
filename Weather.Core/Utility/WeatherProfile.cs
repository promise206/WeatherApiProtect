using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Core.DTOs;
using Weather.Domain.Entities;

namespace Weather.Core.Utility
{
    public class WeatherProfile: Profile
    {
        public WeatherProfile()
        {
            CreateMap<RegistrationDTO, User>()
                 .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email.ToLower()))
                 .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.Email.ToLower()));
            //CreateMap<User, GetUserDTO>().ReverseMap();
        }
    }
}
