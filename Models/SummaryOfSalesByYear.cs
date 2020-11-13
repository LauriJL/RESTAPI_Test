using System;
using System.Collections.Generic;

namespace Restful_Lopputehtava_LauriLeskinen.Models
{
    public partial class SummaryOfSalesByYear
    {
        public DateTime? ShippedDate { get; set; }
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
