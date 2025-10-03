// src/ProductsApi.Tests/ProductDomainTests.cs
using ProductsApi.Domain;
using Xunit;

namespace ProductsApi.Tests
{
    /// <summary>
    /// Testes focados no domínio (entidade Product).
    /// Aqui validamos a propriedade calculada Available.
    /// </summary>
    public class ProductDomainTests
    {
        [Fact]
        public void Available_Should_Be_True_When_Quantity_Greater_Than_Zero()
        {
            // Arrange: estoque com 1 unidade
            var p = new Product { QuantityInStock = 1 };

            // Act + Assert: Available deve ser true
            Assert.True(p.Available);
        }

        [Fact]
        public void Available_Should_Be_False_When_Quantity_Is_Zero()
        {
            // Arrange: estoque zerado
            var p = new Product { QuantityInStock = 0 };

            // Act + Assert: Available deve ser false
            Assert.False(p.Available);
        }
    }
}
