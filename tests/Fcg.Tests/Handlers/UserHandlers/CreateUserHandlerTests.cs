using Fcg.Domain.GameHandlers;
using Fcg.Domain.UserHandlers;
using Microsoft.Extensions.Logging;

namespace Fcg.Tests.Handlers.UserHandlers;

public class CreateUserHandlerTests
{
    [Fact]
    public async Task Handle_DeveCriarUsuarioComSucesso_QuandoEmailNaoExiste()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<CreateUserHandler>>();

        mockRepository
            .Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var handler = new CreateUserHandler(mockRepository.Object, mockLogger.Object);

        var request = new CreateUserRequest(new UserDto
        {
            Name = "João Silva",
            Email = "joao@email.com",
            Password = "Senha@123"
        });

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.message.Should().Be("Usuário cadastrado com sucesso");
        mockRepository.Verify(x => x.CreateUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveHashearSenha_QuandoCriarUsuario()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<CreateUserHandler>>();
        User? usuarioCriado = null;

        mockRepository
            .Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        mockRepository
            .Setup(x => x.CreateUserAsync(It.IsAny<User>()))
            .Callback<User>(user => usuarioCriado = user);

        var handler = new CreateUserHandler(mockRepository.Object, mockLogger.Object);

        var request = new CreateUserRequest(new UserDto
        {
            Name = "João Silva",
            Email = "joao@email.com",
            Password = "Senha@123"
        });

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        usuarioCriado.Should().NotBeNull();
        usuarioCriado!.Password.Should().NotBe("Senha@123");
        usuarioCriado.Password.Should().StartWith("$2a$");
    }

    [Fact]
    public async Task Handle_DeveDefinirRoleComoUser_QuandoCriarUsuario()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<CreateUserHandler>>();
        User? usuarioCriado = null;

        mockRepository
            .Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        mockRepository
            .Setup(x => x.CreateUserAsync(It.IsAny<User>()))
            .Callback<User>(user => usuarioCriado = user);

        var handler = new CreateUserHandler(mockRepository.Object, mockLogger.Object);

        var request = new CreateUserRequest(new UserDto
        {
            Name = "João Silva",
            Email = "joao@email.com",
            Password = "Senha@123"
        });

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        usuarioCriado.Should().NotBeNull();
        usuarioCriado!.Role.Should().Be("User");
    }

    [Fact]
    public async Task Handle_DeveLancarArgumentException_QuandoEmailJaExiste()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<CreateUserHandler>>();

        var usuarioExistente = new User
        {
            Id = Guid.NewGuid(),
            Name = "João Silva",
            Email = "joao@email.com",
            Password = "hash123",
            Role = "User"
        };

        mockRepository
            .Setup(x => x.GetUserByEmailAsync("joao@email.com"))
            .ReturnsAsync(usuarioExistente);

        var handler = new CreateUserHandler(mockRepository.Object, mockLogger.Object);

        var request = new CreateUserRequest(new UserDto
        {
            Name = "Maria Silva",
            Email = "joao@email.com",
            Password = "Senha@123"
        });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => handler.Handle(request, CancellationToken.None)
        );

        exception.Message.Should().Contain("já está cadastrado");
        mockRepository.Verify(x => x.CreateUserAsync(It.IsAny<User>()), Times.Never);
    }
}
