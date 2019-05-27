using System.Collections.Generic;

namespace SalesTaxes.Domain
{
    public class Purchase
    {
        public List<Article> Articles { get; } = new List<Article>();

        public void Add(Article article)
        {
            Articles.Add(article);
        }
    }
}