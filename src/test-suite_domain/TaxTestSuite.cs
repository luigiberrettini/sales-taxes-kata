using SalesTaxesKata.Domain.Taxation;
using Xunit;

namespace SalesTaxesKata.TestSuite.Domain
{
    public class TaxTestSuite
    {
        [Fact]
        public void BasicTaxIsRoundedUpToFiveCents()
        {
            var basicTax = new BasicTax();
            Assert.Equal(2, basicTax.ApplyTo(1.8M));
            Assert.Equal(1.8M, basicTax.ApplyTo(1.6M));
            Assert.Equal(1.65M, basicTax.ApplyTo(1.5M));
            Assert.Equal(1.55M, basicTax.ApplyTo(1.4M));
            Assert.Equal(1.25M, basicTax.ApplyTo(1.1M));
            Assert.Equal(1.1M, basicTax.ApplyTo(1));
        }

        [Fact]
        public void BasicTaxAndImportDutyIsRoundedUpToFiveCents()
        {
            var basicTaxAndImportDuty = new BasicTaxAndImportDuty();
            Assert.Equal(1.9M, basicTaxAndImportDuty.ApplyTo(1.65M));
            Assert.Equal(1.8M, basicTaxAndImportDuty.ApplyTo(1.53M));
            Assert.Equal(3.45M, basicTaxAndImportDuty.ApplyTo(3M));
            Assert.Equal(1.65M, basicTaxAndImportDuty.ApplyTo(1.43M));
            Assert.Equal(1.55M, basicTaxAndImportDuty.ApplyTo(1.31M));
            Assert.Equal(2.3M, basicTaxAndImportDuty.ApplyTo(2));
        }

        [Fact]
        public void ImportDutyIsRoundedUpToFiveCents()
        {
            var importDuty = new ImportDuty();
            Assert.Equal(1.9M, importDuty.ApplyTo(1.8M));
            Assert.Equal(1.3M, importDuty.ApplyTo(1.2M));
            Assert.Equal(1.05M, importDuty.ApplyTo(1));
            Assert.Equal(0.85M, importDuty.ApplyTo(0.8M));
            Assert.Equal(0.25M, importDuty.ApplyTo(0.2M));
            Assert.Equal(21, importDuty.ApplyTo(20));
        }
    }
}