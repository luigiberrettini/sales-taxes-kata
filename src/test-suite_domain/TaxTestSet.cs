using SalesTaxesKata.Domain.Taxation;
using Xunit;

namespace SalesTaxesKata.TestSuite.Domain
{
    public class TaxTestSet
    {
        [Fact]
        public void BasicTaxIsRoundedUpToFiveCents()
        {
            var basicTax = new BasicTax();
            Assert.Equal(1.91M + 0.20M, basicTax.ApplyTo(1.91M));
            Assert.Equal(1.51M + 0.20M, basicTax.ApplyTo(1.51M));
            Assert.Equal(1.50M + 0.15M, basicTax.ApplyTo(1.50M));
            Assert.Equal(1.42M + 0.15M, basicTax.ApplyTo(1.42M));
            Assert.Equal(1.09M + 0.15M, basicTax.ApplyTo(1.09M));
            Assert.Equal(1.00M + 0.10M, basicTax.ApplyTo(1.00M));
        }

        [Fact]
        public void BasicTaxAndImportDutyIsRoundedUpToFiveCents()
        {
            var basicTaxAndImportDuty = new BasicTaxAndImportDuty();
            Assert.Equal(25.30M + 3.80M, basicTaxAndImportDuty.ApplyTo(25.30M));
            Assert.Equal(25.01M + 3.80M, basicTaxAndImportDuty.ApplyTo(25.01M));
            Assert.Equal(25.00M + 3.75M, basicTaxAndImportDuty.ApplyTo(25.00M));
            Assert.Equal(24.95M + 3.75M, basicTaxAndImportDuty.ApplyTo(24.95M));
            Assert.Equal(24.70M + 3.75M, basicTaxAndImportDuty.ApplyTo(24.70M));
            Assert.Equal(24.00M + 3.60M, basicTaxAndImportDuty.ApplyTo(24.00M));
        }

        [Fact]
        public void ImportDutyIsRoundedUpToFiveCents()
        {
            var importDuty = new ImportDuty();
            Assert.Equal(3.82M + 0.20M, importDuty.ApplyTo(3.82M));
            Assert.Equal(3.02M + 0.20M, importDuty.ApplyTo(3.02M));
            Assert.Equal(3.00M + 0.15M, importDuty.ApplyTo(3.00M));
            Assert.Equal(2.84M + 0.15M, importDuty.ApplyTo(2.84M));
            Assert.Equal(2.18M + 0.15M, importDuty.ApplyTo(2.18M));
            Assert.Equal(2.00M + 0.10M, importDuty.ApplyTo(2.00M));
        }
    }
}