using System;
using System.Collections.Generic;
using ProductsApi.Domain;

namespace ProductsApi.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product? GetById(Guid id);
        void Add(Product product);
        void Update(Product product);
        void Delete(Guid id);
    }
}
