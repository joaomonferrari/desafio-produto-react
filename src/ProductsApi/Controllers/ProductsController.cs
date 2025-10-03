using Microsoft.AspNetCore.Mvc;
using ProductsApi.DTOs;
using ProductsApi.Domain;
using ProductsApi.Services;

//CRUD:

namespace ProductsApi.Controllers
{
    // Indica que este controller participa do pipeline Web API:
    // - Faz binding automático (JSON -> objeto)
    // - Retorna 400 automaticamente quando a validação falha (ModelState inválido)
    [ApiController]

    // Define a rota base: "api/products".
    // O token [controller] vira o nome da classe sem "Controller".
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // Dependência do serviço de aplicação.
        // O controller não conhece repositório/banco; ele fala só com a camada de serviço.
        private readonly IProductService _service;

        // O ASP.NET Core injeta IProductService (registrado no Program.cs).
        public ProductsController(IProductService service) => _service = service;

        // GET /api/products
        // Retorna a lista de produtos (200 OK).
        // A serialização para JSON é automática.
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll() => Ok(_service.GetAll());

        // GET /api/products/{id}
        // "{id}" vem da URL e é convertido para Guid automaticamente.
        // Se não achar, devolve 404 NotFound; senão 200 OK com o objeto.
        [HttpGet("{id}")]
        public ActionResult<Product> GetById(Guid id)
        {
            var p = _service.GetById(id);
            return p is null ? NotFound() : Ok(p);
        }

        // POST /api/products
        // Recebe um JSON no corpo da requisição e faz o binding para ProductDto.
        // O [ApiController] + FluentValidation validam o dto antes de chegar aqui;
        // se estiver inválido, o framework retorna 400 automaticamente.
        [HttpPost]
        public ActionResult<Product> Create([FromBody] ProductDto dto)
        {
            // Mapeamento DTO -> Entidade de domínio.
            // Observação: aqui você usa valores default quando algo vier nulo.
            // Na prática, sua validação já garante obrigatoriedade (logo não deveria vir nulo).
            var product = new Product
            {
                Name = dto.Name ?? string.Empty,
                Category = dto.Category ?? string.Empty,
                Price = dto.Price ?? 0M,
                QuantityInStock = dto.QuantityInStock ?? 0,
                CreatedAt = dto.CreatedAt ?? DateTime.UtcNow // salva em UTC
            };

            // Regras de negócio + persistência ficam no service.
            var created = _service.Create(product);

            // 201 Created com header Location apontando para GET /api/products/{id}
            // nameof(GetById) evita "string mágica".
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /api/products/{id}
        // Atualiza um produto existente. A ideia é um "merge parcial":
        // só troca os campos que vierem no DTO (se um campo vier nulo, mantém o atual).
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] ProductDto dto)
        {
            // Busca o atual; se não existir, 404.
            var existing = _service.GetById(id);
            if (existing == null) return NotFound();

            // Aplica alterações campo a campo, preservando o valor atual quando DTO vem nulo.
            existing.Name = dto.Name ?? existing.Name;
            existing.Category = dto.Category ?? existing.Category;
            existing.Price = dto.Price ?? existing.Price;
            existing.QuantityInStock = dto.QuantityInStock ?? existing.QuantityInStock;

            // Chama a regra de negócio para persistir.
            _service.Update(existing);

            // 204 No Content: atualização concluída sem corpo na resposta.
            return NoContent();
        }

        // DELETE /api/products/{id}
        // Remove o produto. Se o id não existir, responde 404.
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var existing = _service.GetById(id);
            if (existing == null) return NotFound();

            _service.Delete(id);
            return NoContent(); // 204
        }
    }
}
