using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Models.Orders
{
    public class CartListViewModel
    {
        [JsonProperty("OrderDataViewModel")]
        public CartViewModel CartViewModel { get; set; }

        [JsonProperty("ListTextOrderDataViewModel")]
        public ListTextOrderDataViewModel ListTextOrderDataViewModel { get; set; }

    }
}
