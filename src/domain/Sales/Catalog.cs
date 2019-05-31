using System;
using System.Collections.Generic;
using System.Linq;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Shopping;

namespace SalesTaxesKata.Domain.Sales
{
    public class Catalog
    {
        private readonly IDictionary<string, IDictionary<int, Article>> _articles;

        public Catalog()
        {
            _articles = new Dictionary<string, IDictionary<int, Article>>();
        }

        public bool TryAdd(Article article)
        {
            if (!_articles.ContainsKey(article.Name))
                _articles[article.Name] = new Dictionary<int, Article>();

            if (_articles[article.Name].ContainsKey(article.Id))
                return false;
            _articles[article.Name][article.Id] = article;
            return true;
        }

        public Article Find(string name, Origin origin, Country saleCountry)
        {
            bool IsFromSaleCountry(Article a) => a.SupplierCountry == saleCountry;
            var mustBeLocal = origin == Origin.Local;

            // Rely on dictionary and Linq exceptions
            var articles = _articles[name].Values;
            return articles.Single(x => IsFromSaleCountry(x) == mustBeLocal);
        }
    }
}