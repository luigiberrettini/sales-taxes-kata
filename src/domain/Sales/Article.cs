using SalesTaxesKata.Domain.Geo;

namespace SalesTaxesKata.Domain.Sales
{
    public class Article
    {
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