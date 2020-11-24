using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Data
{
   public class UserSessionCartDto
    {
        public string UserSessionId { get; set; }
        public int Count { get; set; }
        public string Size { get; set; }
        public string DesignFolderPath { get; set; }
        public string DesignName { get; set; }
        public double Price { get; set; }
        public string ProductId { get; set; }
        public double Sum { get; set; }
    }
}
