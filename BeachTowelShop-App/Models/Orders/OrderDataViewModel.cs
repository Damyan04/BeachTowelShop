﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Models.Orders
{
    public class OrderDataViewModel
    {
        public string SessionId { get; set; }
        public int Count { get; set; }
        public string Size { get; set; }
        public string DesignFolderPath { get; set; }
        public string DesignName { get; set; }
        public double Price { get; set; }
        public string ProductId { get; set; }

        public double Sum { get; set; }
    }
}