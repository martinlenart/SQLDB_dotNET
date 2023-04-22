using System;
namespace CustomerOrderModel
{
    public class csOrderDetail
	{
        public csArticle Article { get; set; }

        public int NrOfArticlesOrdered { get; set; }
        public decimal ValueOfArticlesOrdered { get; set; }
	}
}

