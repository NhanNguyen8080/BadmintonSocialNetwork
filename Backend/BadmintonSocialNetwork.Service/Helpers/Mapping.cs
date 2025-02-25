using AutoMapper;
using BadmintonSocialNetwork.Repository.Models;
using BadmintonSocialNetwork.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Service.Helpers
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            #region Account
            CreateMap<Account, AccountVM>().ReverseMap();
            CreateMap<Account, AccountCM>().ReverseMap();
            CreateMap<Account, AccountUM>().ReverseMap();
            #endregion
        }
    }
}
