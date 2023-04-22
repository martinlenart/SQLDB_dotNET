using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerOrderModel
{
	public class csOrder
	{
        public DateTime OrderDate { get; private set; }
        public DateTime? DeliveryDate { get; set; }

        public List<csOrderDetail> OrderDetails { get; set; } = new List<csOrderDetail>();
	}
}

