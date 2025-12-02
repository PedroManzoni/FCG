using Fcg.Shareable.Dtos;
using FluentValidation;

namespace Fcg.Shareable.Validators;

public sealed class GameDtoValidator : AbstractValidator<GameDto>
{
    public GameDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do jogo é obrigatório.")
            .MinimumLength(2).WithMessage("O nome deve ter no mínimo 2 caracteres.")
            .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição do jogo é obrigatório.")
            .MaximumLength(500).WithMessage("A descrição não pode exceder 500 caracteres.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("O preço não pode ser negativo.")
            .LessThanOrEqualTo(9999.99m).WithMessage("O preço não pode exceder R$ 9.999,99.");
    }
}