using Fcg.Shareable.Validators;

namespace Fcg.Tests.Validators;

public class UserDtoValidatorTests
{
    private readonly UserDtoValidator _validator;

    public UserDtoValidatorTests()
    {
        _validator = new UserDtoValidator();
    }

    [Fact]
    public void Validate_DevePassar_QuandoDadosValidos()
    {
        // Arrange
        var dto = new UserDto
        {
            Name = "João Silva",
            Email = "joao@email.com",
            Password = "Senha@123"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoNomeVazio()
    {
        // Arrange
        var dto = new UserDto
        {
            Name = "",
            Email = "joao@email.com",
            Password = "Senha@123"
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
        var dto = new UserDto
        {
            Name = "Jo",
            Email = "joao@email.com",
            Password = "Senha@123"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage.Contains("mínimo 3"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoEmailVazio()
    {
        // Arrange
        var dto = new UserDto
        {
            Name = "João Silva",
            Email = "",
            Password = "Senha@123"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoEmailInvalido()
    {
        // Arrange
        var dto = new UserDto
        {
            Name = "João Silva",
            Email = "email-invalido",
            Password = "Senha@123"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoSenhaVazia()
    {
        // Arrange
        var dto = new UserDto
        {
            Name = "João Silva",
            Email = "joao@email.com",
            Password = ""
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoSenhaMuitoCurta()
    {
        // Arrange
        var dto = new UserDto
        {
            Name = "João Silva",
            Email = "joao@email.com",
            Password = "Abc@1"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" && e.ErrorMessage.Contains("mínimo 8"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoSenhaSemMaiuscula()
    {
        // Arrange
        var dto = new UserDto
        {
            Name = "João Silva",
            Email = "joao@email.com",
            Password = "senha@123"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" && e.ErrorMessage.Contains("maiúscula"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoSenhaSemMinuscula()
    {
        // Arrange
        var dto = new UserDto
        {
            Name = "João Silva",
            Email = "joao@email.com",
            Password = "SENHA@123"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" && e.ErrorMessage.Contains("minúscula"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoSenhaSemNumero()
    {
        // Arrange
        var dto = new UserDto
        {
            Name = "João Silva",
            Email = "joao@email.com",
            Password = "Senha@abc"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" && e.ErrorMessage.Contains("número"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoSenhaSemCaractereEspecial()
    {
        // Arrange
        var dto = new UserDto
        {
            Name = "João Silva",
            Email = "joao@email.com",
            Password = "Senha123"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" && e.ErrorMessage.Contains("especial"));
    }
}
