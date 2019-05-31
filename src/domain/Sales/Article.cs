using SalesTaxesKata.Domain.Geo;

namespace SalesTaxesKata.Domain.Sales
{
    public class Article
    {
        public static readonly Article NullArticle = new Article(0, Country.NullCountry, 0, string.Empty, 0);

        public int Id { get; }

        public Country SupplierCountry { get; }

        public Category Category { get; }

        public string Name { get; }

        public decimal Price { get; }

        public Article(int id, Country supplierCountry, Category category, string name, decimal price)
        {
            Id = id;
            SupplierCountry = supplierCountry;
            Category = category;
            Name = name;
            Price = price;
        }
    }
}