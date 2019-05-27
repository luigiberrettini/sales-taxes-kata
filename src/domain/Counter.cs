using System.Collections.Generic;

namespace SalesTaxes.Domain
{
    public class Counter
    {
        private readonly List<Article> _purchase = new List<Article>();

        public void Scan(Article article)
        {
            _purchase.Add(article);
        }

        public Receipt EmitReceipt()
        {
            return new Receipt(_purchase);
        }
    }
}