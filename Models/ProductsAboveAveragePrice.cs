using System;
using System.Collections.Generic;

namespace Restful_Lopputehtava_LauriLeskinen.Models
{
    public partial class ProductsAboveAveragePrice
    {
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
