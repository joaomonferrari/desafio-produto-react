// src/ProductsApi.Tests/ProductsServiceTests.cs
using System;
using System.Linq;
using ProductsApi.Domain;
using ProductsApi.Repositories;
using ProductsApi.Services;
using Xunit;

namespace ProductsApi.Tests
{
    /// <summary>
    /// Testes de UNIDADE para a camada de serviço (ProductService).
    /// Segue o padrão AAA (Arrange -> Act -> Assert).
    /// </summary>
    public class ProductsServiceTests
    {
        [Fact]
        public void CreateProduct_Should_Set_Id_And_CreatedAtUtc_And_Persist()
        {
            // Arrange: repositório em memória + service
            var repo = new InMemoryProductRepository();
            var service = new ProductService(repo);

            // Produto de entrada (sem Id definido manualmente)
            var product = new Product
            {
                Name = "Notebook",
                Category = "Eletrônicos",
                Price = 3999.90m,
                QuantityInStock = 3
            };

            // Act: cria o produto
            var created = service.Create(product);

            // Assert:
            // - Id deve ser diferente de Guid.Empty (foi gerado)
            Assert.NotEqual(Guid.Empty, created.Id);

            // - CreatedAt deve estar em UTC (boa prática em APIs)
            Assert.Equal(DateTimeKind.Utc, created.CreatedAt.Kind);

            // - Produto deve ter sido persistido no repositório
            //   (Assert.Contains é preferível a Assert.True(Any(...)) pois dá mensagem melhor)
            Assert.Contains(repo.GetAll(), p => p.Id == created.Id);
        }

        [Fact]
        public void UpdateProduct_Should_Change_Fields()
        {
            // Arrange: cria item inicial
            var repo = new InMemoryProductRepository();
            var service = new ProductService(repo);

            var p = service.Create(new Product
            {
                Name = "Mouse",
                Category = "Periféricos",
                Price = 99.90m,
                QuantityInStock = 10
            });

            // Act: altera propriedades e atualiza
            p.Name = "Mouse Gamer";
            p.Price = 149.90m;
            p.QuantityInStock = 7;
            service.Update(p);

            // Assert: recarrega e confere alterações
            var fromRepo = service.GetById(p.Id)!;
            Assert.Equal("Mouse Gamer", fromRepo.Name);
            Assert.Equal(149.90m, fromRepo.Price);
            Assert.Equal(7, fromRepo.QuantityInStock);
        }

        [Fact]
        public void UpdateProduct_Should_Throw_When_NotFound()
        {
            // Arrange: produto que NÃO existe no repositório (Id aleatório)
            var repo = new InMemoryProductRepository();
            var service = new ProductService(repo);

            var phantom = new Product
            {
                Id = Guid.NewGuid(),               // id inexistente
                Name = "Fantasma",
                Category = "N/A",
                Price = 1m,
                QuantityInStock = 1,
                CreatedAt = DateTime.UtcNow
            };

            // Act + Assert: service.Update deve lançar exceção de "não encontrado"
            var ex = Assert.Throws<Exception>(() => service.Update(phantom));
            Assert.Equal("Produto não encontrado", ex.Message);
        }

        [Fact]
        public void DeleteProduct_Should_Remove()
        {
            // Arrange: cria item e garante que está no repositório
            var repo = new InMemoryProductRepository();
            var service = new ProductService(repo);

            var p = service.Create(new Product
            {
                Name = "Teclado",
                Category = "Periféricos",
                Price = 199.90m,
                QuantityInStock = 5
            });

            // Act: remove o item
            service.Delete(p.Id);

            // Assert: GetById deve retornar null (não existe mais)
            Assert.Null(service.GetById(p.Id));
        }

        [Fact]
        public void GetById_Should_Return_Null_When_NotFound()
        {
            // Arrange: service em memória
            var repo = new InMemoryProductRepository();
            var service = new ProductService(repo);

            // Act: busca por um Guid inexistente
            var result = service.GetById(Guid.NewGuid());

            // Assert: deve ser null
            Assert.Null(result);
        }

        [Fact]
        public void GetAll_Should_Include_Newly_Created_Item()
        {
            // Arrange
            var repo = new InMemoryProductRepository();
            var service = new ProductService(repo);

            // O repositório já tem 1 item seed; contamos antes
            var before = repo.GetAll().Count();

            // Act: cria novo item
            service.Create(new Product
            {
                Name = "Headset",
                Category = "Periféricos",
                Price = 299.90m,
                QuantityInStock = 2
            });

            // Assert: total deve ter aumentado em 1
            var after = repo.GetAll().Count();
            Assert.Equal(before + 1, after);
        }
    }
}
