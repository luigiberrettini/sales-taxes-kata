using SalesTaxes.Domain.Catalog;
using SalesTaxes.Domain.Payment;
using System.Collections.Generic;

namespace SalesTaxes.Domain.Shopping
{
    public class Purchase
    {
        public List<Article> Articles { get; } = new List<Article>();

        public void Add(Article article)
        {
            Articles.Add(article);
        }

        public Receipt BuildReceipt()
        {
            return new Receipt(this);
        }
    }
}