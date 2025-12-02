using Fcg.Shareable.Validators;
using System.ComponentModel.DataAnnotations;

namespace Fcg.Tests.Validators;

public class GameDtoValidatorTests
{
    private readonly GameDtoValidator _validator;

    public GameDtoValidatorTests()
    {
        _validator = new GameDtoValidator();
    }

    [Fact]
    public void Validate_DevePassar_QuandoDadosValidos()
    {
        // Arrange
        var dto = new GameDto
        {
            Name = "God of War",
            Description = "Jogo de ação",
            Price = 199.90m
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoNomeVazio()
    {
        // Arrange
        var dto = new GameDto
        {
            Name = "",
            Description = "Jogo de ação",
            Price = 199.90m
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoNomeMuitoCurto()
    {
        // Arrange
        var dto = new GameDto
        {
            Name = "A",
            Description = "Jogo de ação",
            Price = 199.90m
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoPrecoNegativo()
    {
        // Arrange
        var dto = new GameDto
        {
            Name = "God of War",
            Description = "Jogo de ação",
            Price = -50m
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Price");
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoDescricaoVazia()
    {
        // Arrange
        var dto = new GameDto
        {
            Name = "God of War",
            Description = "",
            Price = 199.90m
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
    }
}