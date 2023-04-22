using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerOrderModel
{
	public class csCustomer
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Adress { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public List<csOrder> Orders { get; set; } = new List<csOrder>();
	}
}

