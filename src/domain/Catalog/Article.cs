namespace SalesTaxes.Domain.Catalog
{
    public class Article
    {
        public int Id { get; }

        public string Name { get; }

        public decimal Price { get; }

        public Article(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}