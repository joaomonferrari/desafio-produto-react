using FluentValidation;
using ProductsApi.DTOs;

namespace ProductsApi.Validators
{
    /// <summary>
    /// Validador do ProductDto usando FluentValidation.
    /// Observação: com AddFluentValidationAutoValidation() configurado no Program.cs,
    /// o ASP.NET Core executa este validador automaticamente antes da action do controller.
    /// Se alguma regra falhar, a API responde 400 (BadRequest) com os detalhes do erro
    /// e a action não é executada.
    /// </summary>
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            // Nome é obrigatório: string não pode ser nula nem vazia/whitespace.
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nome é obrigatório");

            // Categoria é obrigatória.
            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Categoria é obrigatória");

            // Preço é obrigatório e não pode ser negativo.
            // NotNull() garante que veio no JSON; GreaterThanOrEqualTo(0) impede valores < 0.
            RuleFor(x => x.Price)
                .NotNull().WithMessage("Preço é obrigatório")
                .GreaterThanOrEqualTo(0).WithMessage("Preço não pode ser negativo");

            // Quantidade em estoque é obrigatória e não pode ser negativa.
            RuleFor(x => x.QuantityInStock)
                .NotNull().WithMessage("Quantidade em estoque é obrigatória")
                .GreaterThanOrEqualTo(0).WithMessage("Quantidade não pode ser negativa");

            // Data de inclusão é obrigatória.
            // (No controller/serviço você pode normalizar para UTC quando necessário.)
            RuleFor(x => x.CreatedAt)
                .NotNull()
                .WithMessage("Data de inclusão é obrigatória");
        }
    }
}
