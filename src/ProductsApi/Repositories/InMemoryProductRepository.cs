using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ProductsApi.Domain;

//CRUD:

namespace ProductsApi.Repositories
{
    // Implementação de IProductRepository armazenando dados em memória.
    // A classe é registrada como Singleton no DI, então o _store vive enquanto a API estiver rodando.
    public class InMemoryProductRepository : IProductRepository
    {
        // ConcurrentDictionary permite leituras/escritas de múltiplas requisições em paralelo
        // com segurança básica de thread.
        private readonly ConcurrentDictionary<Guid, Product> _store = new();

        public InMemoryProductRepository()
        {
            // "Seed" inicial: adiciona um produto de exemplo.
            var p = new Product
            {
                Name = "Exemplo",
                Category = "Eletrônicos",
                Price = 199.90M,
                QuantityInStock = 10
                // Id e CreatedAt têm defaults na entidade (Guid.NewGuid e UtcNow)
            };
            _store[p.Id] = p; // Upsert: se a chave existir, sobrescreve; se não, cria.
        }

        // Retorna todos os produtos.
        // Observação: _store.Values é uma view "viva" que pode mudar enquanto você itera.
        // Para APIs simples ok; para estabilidade, poderia materializar ToList().
        public IEnumerable<Product> GetAll() => _store.Values;

        // Busca por Id. TryGetValue evita exceção e retorna null quando não encontra.
        public Product? GetById(Guid id) => _store.TryGetValue(id, out var p) ? p : null;

        // Adiciona ou substitui o produto com a mesma chave.
        // Aqui usamos a semântica de upsert (_store[id] = obj).
        public void Add(Product product) => _store[product.Id] = product;

        // Atualiza (upsert). Em conjunto com o Service, significa "gravar o estado final".
        public void Update(Product product) => _store[product.Id] = product;

        // Remove pelo Id (ignora se não existir).
        public void Delete(Guid id) => _store.TryRemove(id, out _);
    }
}
