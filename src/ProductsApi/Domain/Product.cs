using System;

namespace ProductsApi.Domain
{
    /// <summary>
    /// Entidade de domínio que representa um Produto.
    /// Não conhece detalhes de web, banco ou validação; apenas o modelo de negócio.
    /// </summary>
    public class Product
    {
        // Identificador único do produto (GUID).
        // É inicializado com um novo Guid por padrão ao criar o objeto.
        public Guid Id { get; set; } = Guid.NewGuid();

        // Nome do produto.
        // Regras como "não vazio" são validadas fora (FluentValidation no DTO).
        public string Name { get; set; } = string.Empty;

        // Categoria do produto (ex.: "Eletrônicos", "Roupas"...).
        public string Category { get; set; } = string.Empty;

        // Preço unitário do produto.
        // "decimal" é o tipo recomendado para valores monetários.
        public decimal Price { get; set; }

        // Quantidade disponível em estoque (inteiro).
        public int QuantityInStock { get; set; }

        // Data/hora de criação do registro (UTC por padrão).
        // Mantida aqui para auditoria/consulta simples.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Propriedade somente leitura (calculada).
        // Retorna true quando há pelo menos 1 unidade em estoque.
        public bool Available => QuantityInStock > 0;
    }
}
