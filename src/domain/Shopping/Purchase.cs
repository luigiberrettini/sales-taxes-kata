using System.Collections.Generic;
using SalesTaxes.Domain.Catalog;

namespace SalesTaxes.Domain.Shopping
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