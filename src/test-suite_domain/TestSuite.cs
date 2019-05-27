using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SalesTaxes.TestSuite.Domain
{
    public class TestSuite
    {
        [Fact]
        public void OneBookIsExempt()
        {
            var counter = new Counter();
            var article = new Article { Id = 1, Name = "Gone with the wind", Price = 25.0M };
            counter.Scan(article);
            var receipt = counter.EmitReceipt();
            Assert.Equal(article.Price, receipt.Entries.SingleOrDefault()?.Price);
        }
    }

    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class Counter
    {
        public void Scan(Article article)
        {
        }

        public Receipt EmitReceipt()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Receipt
    {
        public IEnumerable<Entry> Entries { get; set; }
    }

    public class Entry
    {
        public decimal Price { get; set; }
    }
}