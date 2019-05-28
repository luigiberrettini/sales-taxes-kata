namespace SalesTaxesKata.Domain.Catalog
{
    public class Article
    {
        public int Id { get; }

        public Supplier Supplier { get; set; }

        public Category Category { get; }

        public string Name { get; }

        public decimal Price { get; }


        public Article(int id, Category category, string name, decimal price) : this(id, new Supplier(string.Empty, string.Empty, Country.Abw), category, name, price)
        {
        }

        public Article(int id, Supplier supplier, Category category, string name, decimal price)
        {
            Id = id;
            Supplier = supplier;
            Category = category;
            Name = name;
            Price = price;
        }
    }
}