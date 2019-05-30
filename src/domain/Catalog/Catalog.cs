using System.Collections.Generic;

namespace SalesTaxesKata.Domain.Catalog
{
    public class Catalog
    {
        private readonly IDictionary<string, Article> _articles;

        public IReadOnlyCollection<Article> Articles => (IReadOnlyCollection<Article>)_articles.Values;

        public Catalog()
        {
            _articles = new Dictionary<string, Article>();
        }

        public void Add(Article article)
        {
            // Rely on dictionary exceptions
            _articles.Add(article.Name, article);
        }

        public Article Find(string name)
        {
            // Rely on dictionary exceptions
            return _articles[name];
        }
    }
}