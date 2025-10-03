using System;

namespace ProductsApi.DTOs
{
    /// <summary>
    /// DTO (Data Transfer Object) usado na borda HTTP para entrada/saída da API.
    /// Mantém o contrato com o cliente desacoplado da entidade de domínio (Product).
    /// As propriedades são anuláveis para facilitar criação/atualização parcial via JSON.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Identificador do produto.
        /// Nulo em criação (POST); preenchido em respostas e usado no caminho do recurso (GET/PUT/DELETE).
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Nome do produto.
        /// Validado pelo FluentValidation no pipeline (obrigatório em criação/atualização conforme regra).
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Categoria do produto (ex.: "Eletrônicos", "Roupas").
        /// Também validada pelo FluentValidation.
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Preço unitário do produto.
        /// Usamos decimal para valores monetários; validação garante não-negativo.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Quantidade disponível em estoque.
        /// Validação impede valores negativos.
        /// </summary>
        public int? QuantityInStock { get; set; }

        /// <summary>
        /// Data/hora de criação do registro (esperado em UTC).
        /// Na criação, o backend pode definir um valor padrão (DateTime.UtcNow) se não for enviado.
        /// </summary>
        public DateTime? CreatedAt { get; set; }
    }
}
