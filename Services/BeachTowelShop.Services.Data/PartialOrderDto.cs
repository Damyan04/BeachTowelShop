using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Data
{
    public class PartialOrderDto
    {
        public string Id { get; set; }

        public double Sum { get; set; }

        public string Status { get; set; }
    }
}
