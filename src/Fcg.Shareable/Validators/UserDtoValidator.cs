using Fcg.Shareable.Dtos;
using Fcg.Shareable.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fcg.Shareable.Validators;
public sealed class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do usuário é obrigatório.")
            .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.")
            .MaximumLength(100).WithMessage("O email não pode exceder 100 caracteres.")
            .Must(BeAValidEmailFormat).WithMessage("O formato do email é inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.")
            .MaximumLength(100).WithMessage("A senha não pode exceder 100 caracteres.")
            .Must(ContainUppercase).WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
            .Must(ContainLowercase).WithMessage("A senha deve conter pelo menos uma letra minúscula.")
            .Must(ContainDigit).WithMessage("A senha deve conter pelo menos um número.")
            .Must(ContainSpecialCharacter).WithMessage("A senha deve conter pelo menos um caractere especial (@, #, $, %, etc).");
    }

    private bool BeAValidEmailFormat(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailRegex);
    }

    private bool ContainUppercase(string password)
    {
        return !string.IsNullOrWhiteSpace(password) && password.Any(char.IsUpper);
    }

    private bool ContainLowercase(string password)
    {
        return !string.IsNullOrWhiteSpace(password) && password.Any(char.IsLower);
    }

    private bool ContainDigit(string password)
    {
        return !string.IsNullOrWhiteSpace(password) && password.Any(char.IsDigit);
    }

    private bool ContainSpecialCharacter(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        var specialCharacters = "!@#$%^&*()_+-=[]{}|;:,.<>?";
        return password.Any(c => specialCharacters.Contains(c));
    }
}
