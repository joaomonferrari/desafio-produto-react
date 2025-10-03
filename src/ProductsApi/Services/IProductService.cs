using System;
using System.Collections.Generic;
using ProductsApi.Domain;

namespace ProductsApi.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product? GetById(Guid id);
        Product Create(Product product);
        void Update(Product product);
        void Delete(Guid id);
    }
}
