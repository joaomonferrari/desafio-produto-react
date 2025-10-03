using System;
using System.Collections.Generic;
using ProductsApi.Domain;
using ProductsApi.Repositories;

//CRUD:

namespace ProductsApi.Services
{
    // Serviço de aplicação: concentra regras de negócio e
    // orquestra o acesso ao repositório (persistência).
    public class ProductService : IProductService
    {
        // Dependência do repositório (abstração de dados).
        private readonly IProductRepository _repo;

        // Injeção de dependência via construtor.
        public ProductService(IProductRepository repo) => _repo = repo;

        // Retorna todos os produtos.
        // Aqui não há regra adicional, apenas delega ao repositório.
        public IEnumerable<Product> GetAll() => _repo.GetAll();

        // Busca por ID. Retorna null se não encontrar.
        public Product? GetById(Guid id) => _repo.GetById(id);

        // Cria um novo produto.
        public Product Create(Product product)
        {
            // Regras mínimas de criação:
            // - Gera um novo Id (garante unicidade)
            product.Id = Guid.NewGuid();

            // - Normaliza a data de criação para UTC (boa prática em APIs)
            product.CreatedAt = DateTime.UtcNow;

            // Persiste no repositório
            _repo.Add(product);

            // Retorna o objeto já persistido (com Id/CreatedAt definidos)
            return product;
        }

        // Atualiza um produto existente.
        public void Update(Product product)
        {
            // Garante que o item existe antes de atualizar.
            var existing = _repo.GetById(product.Id);
            if (existing == null) throw new Exception("Produto não encontrado");

            // Observação:
            // Aqui você está sobrescrevendo com o 'product' recebido.
            // Como o controller já fez o merge campo a campo,
            // este 'product' já representa o estado final.
            _repo.Update(product);
        }

        // Remove um produto pelo ID.
        public void Delete(Guid id) => _repo.Delete(id);
        // Observação:
        // O controller já verifica existência antes de chamar Delete.
        // Se quiser deixar o service mais robusto, você pode conferir
        // existência aqui também e lançar exceção quando não houver.
    }
}
