using System;
namespace CustomerOrderModel
{
    public enum enArticleType { Movie, Book, Game }
    public class csArticle
	{
        public enArticleType ArticleType { get; set; }
		public string ArticleTitle { get; set; }
		public decimal ArticlePrice { get; set; }
	}
}

