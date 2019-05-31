using System;
using System.Collections.Generic;
using System.Linq;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Shopping;

namespace SalesTaxesKata.Domain.Sales
{
    public class Catalog
    {
        private readonly IDictionary<string, IDictionary<int, Article>> _byNameAndId;

        public Catalog()
        {
            _byNameAndId = new Dictionary<string, IDictionary<int, Article>>();
        }

        public (bool, Article) TryAdd(Article article)
        {
            if (!_byNameAndId.ContainsKey(article.Name))
                _byNameAndId[article.Name] = new Dictionary<int, Article>();

            return _byNameAndId[article.Name].ContainsKey(article.Id) ?
                (false, _byNameAndId[article.Name][article.Id]) :
                (true, _byNameAndId[article.Name][article.Id] = article);
        }

        public Article Find(string name, Origin origin, Country saleCountry)
        {
            bool MatchesOrigin(Article a)
            {
                var isFromSaleCountry = a.SupplierCountry == saleCountry;
                var mustBeLocal = origin == Origin.Local;
                return isFromSaleCountry == mustBeLocal;
            }
            var matchingName = _byNameAndId.ContainsKey(name) ? _byNameAndId[name].Values : Enumerable.Empty<Article>();
            return matchingName.SingleOrDefault(MatchesOrigin) ?? Article.NullArticle;
        }
    }
}