using Fcg.Domain.UserHandlers;
using Microsoft.Extensions.Logging;

namespace Fcg.Tests.Handlers.UserHandlers;

public class UpdateUserHandlerTests
{
    [Fact]
    public async Task Handle_DeveAtualizarNome_QuandoUsuarioExiste()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<UpdateUserHandler>>();

        var usuarioExistente = new User
        {
            Id = Guid.NewGuid(),
            Name = "João Silva",
            Email = "joao@email.com",
            Password = "hash123",
            Role = "User",
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            LastUpdatedAt = DateTime.UtcNow.AddDays(-10)
        };

        mockRepository
            .Setup(x => x.GetUserByEmailAsync("joao@email.com"))
            .ReturnsAsync(usuarioExistente);

        var handler = new UpdateUserHandler(mockRepository.Object, mockLogger.Object);

        var request = new UpdateUserRequest(
            "joao@email.com",
            new UserDto
            {
                Name = "João Pedro Silva",
                Email = "joao@email.com",
                Password = ""
            }
        );

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.message.Should().Be("Usuário alterado com sucesso");
        mockRepository.Verify(x => x.UpdateUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveHashearNovaSenha_QuandoAtualizarSenha()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<UpdateUserHandler>>();
        User? usuarioAtualizado = null;

        var usuarioExistente = new User
        {
            Id = Guid.NewGuid(),
            Name = "João Silva",
            Email = "joao@email.com",
            Password = "$2a$11$senhaantigahash",
            Role = "User"
        };

        mockRepository
            .Setup(x => x.GetUserByEmailAsync("joao@email.com"))
            .ReturnsAsync(usuarioExistente);

        mockRepository
            .Setup(x => x.UpdateUserAsync(It.IsAny<User>()))
            .Callback<User>(user => usuarioAtualizado = user);

        var handler = new UpdateUserHandler(mockRepository.Object, mockLogger.Object);

        var request = new UpdateUserRequest(
            "joao@email.com",
            new UserDto
            {
                Name = "João Silva",
                Email = "joao@email.com",
                Password = "NovaSenha@123"
            }
        );

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        usuarioAtualizado.Should().NotBeNull();
        usuarioAtualizado!.Password.Should().NotBe("NovaSenha@123");
        usuarioAtualizado.Password.Should().NotBe("$2a$11$senhaantigahash"); 
        usuarioAtualizado.Password.Should().StartWith("$2a$"); 
    }

    [Fact]
    public async Task Handle_NaoDeveAtualizarSenha_QuandoSenhaVazia()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<UpdateUserHandler>>();
        User? usuarioAtualizado = null;

        var senhaOriginal = "$2a$11$senhaoriginalhash";

        var usuarioExistente = new User
        {
            Id = Guid.NewGuid(),
            Name = "João Silva",
            Email = "joao@email.com",
            Password = senhaOriginal,
            Role = "User"
        };

        mockRepository
            .Setup(x => x.GetUserByEmailAsync("joao@email.com"))
            .ReturnsAsync(usuarioExistente);

        mockRepository
            .Setup(x => x.UpdateUserAsync(It.IsAny<User>()))
            .Callback<User>(user => usuarioAtualizado = user);

        var handler = new UpdateUserHandler(mockRepository.Object, mockLogger.Object);

        var request = new UpdateUserRequest(
            "joao@email.com",
            new UserDto
            {
                Name = "João Pedro Silva",
                Email = "joao@email.com",
                Password = ""
            }
        );

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        usuarioAtualizado.Should().NotBeNull();
        usuarioAtualizado!.Password.Should().Be(senhaOriginal);
    }

    [Fact]
    public async Task Handle_DeveLancarKeyNotFoundException_QuandoUsuarioNaoExiste()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<UpdateUserHandler>>();

        mockRepository
            .Setup(x => x.GetUserByEmailAsync("inexistente@email.com"))
            .ReturnsAsync((User?)null);

        var handler = new UpdateUserHandler(mockRepository.Object, mockLogger.Object);

        var request = new UpdateUserRequest(
            "inexistente@email.com",
            new UserDto
            {
                Name = "Nome",
                Email = "inexistente@email.com",
                Password = "Senha@123"
            }
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => handler.Handle(request, CancellationToken.None)
        );

        exception.Message.Should().Contain("não encontrado");
        mockRepository.Verify(x => x.UpdateUserAsync(It.IsAny<User>()), Times.Never);
    }
}
