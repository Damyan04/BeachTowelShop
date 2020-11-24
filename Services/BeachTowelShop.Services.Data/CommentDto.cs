using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Data
{
   public class CommentDto
    {
        public string Id { get; set; }
        public string Description { get; set; } = null;
        
        public string ProductId { get; set; }
    }
}
