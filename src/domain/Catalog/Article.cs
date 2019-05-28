namespace SalesTaxesKata.Domain.Catalog
{
    public class Article
    {
        public int Id { get; }

        public Category Category { get; }

        public string Name { get; }

        public decimal Price { get; }

        public Article(int id, Category category, string name, decimal price)
        {
            Id = id;
            Category = category;
            Name = name;
            Price = price;
        }
    }
}