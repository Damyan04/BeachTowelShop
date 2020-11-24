using BeachTowelShop.Models.Comments;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Automapper
{
    public class CommentViewModelProfile : AutoMapper.Profile
    {
        public CommentViewModelProfile()
        {
            CreateMap<CommentDto, CommentViewModel>().ForMember(s => s.Description, t => t.MapFrom(src => src.Description)).ReverseMap();
        }
    }
}
