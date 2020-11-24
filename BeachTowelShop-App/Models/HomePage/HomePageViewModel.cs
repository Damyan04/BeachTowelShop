using BeachTowelShop.Models.Comments;
using BeachTowelShop.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Models.HomePage
{
    public class HomePageViewModel
    {
        public HomePageViewModel()
        {
            Towels = new List<HomePageTowelsViewModel>();
            Comments = new List<CommentViewModel>();
        }
        public List <HomePageTowelsViewModel> Towels { get; set; }
        public List <CommentViewModel> Comments { get; set; }
    }
}
