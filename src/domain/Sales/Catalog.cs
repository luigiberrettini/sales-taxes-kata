using System.Collections.Generic;
using System.Linq;
using SalesTaxesKata.Domain.Geo;

namespace SalesTaxesKata.Domain.Sales
{
    public class Catalog
    {
        private readonly IDictionary<string, IDictionary<int, Article>> _articles;

        public Catalog()
        {
            _articles = new Dictionary<string, IDictionary<int, Article>>();
        }

        public void Add(Article article)
        {
            if (!_articles.ContainsKey(article.Name))
                _articles[article.Name] = new Dictionary<int, Article>();
            // Rely on dictionary exceptions
            _articles[article.Name].Add(article.Id, article);
        }

        public Article Find(string name, bool isImported, Country saleCountry)
        {
            // Rely on dictionary and Linq exceptions
            var articles = _articles[name].Values;
            return articles.Single(x => (x.SupplierCountry != saleCountry) == isImported);

        }
    }
}