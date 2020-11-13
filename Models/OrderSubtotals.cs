using System;
using System.Collections.Generic;

namespace Restful_Lopputehtava_LauriLeskinen.Models
{
    public partial class OrderSubtotals
    {
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
