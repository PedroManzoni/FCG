using Fcg.Domain.UserHandlers;
using Microsoft.Extensions.Logging;

namespace Fcg.Tests.Handlers.UserHandlers;

public class GetUserByEmailHandlerTests
{
    [Fact]
    public async Task Handle_DeveRetornarUsuario_QuandoUsuarioExiste()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<GetUserByEmailHandler>>();

        var usuarioExistente = new User
        {
            Id = Guid.NewGuid(),
            Name = "João Silva",
            Email = "joao@email.com",
            Password = "hash123",
            Role = "User",
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        mockRepository
            .Setup(x => x.GetUserByEmailAsync("joao@email.com"))
            .ReturnsAsync(usuarioExistente);

        var handler = new GetUserByEmailHandler(mockRepository.Object, mockLogger.Object);
        var request = new GetUserByEmailRequest("joao@email.com");

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.user.Should().NotBeNull();
        response.user.Name.Should().Be("João Silva");
        response.user.Email.Should().Be("joao@email.com");
    }

    [Fact]
    public async Task Handle_NaoDeveRetornarSenha_QuandoBuscarUsuario()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<GetUserByEmailHandler>>();

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

        var handler = new GetUserByEmailHandler(mockRepository.Object, mockLogger.Object);
        var request = new GetUserByEmailRequest("joao@email.com");

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.user.Should().NotBeNull();
        response.user.Password.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_DeveLancarKeyNotFoundException_QuandoUsuarioNaoExiste()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<GetUserByEmailHandler>>();

        mockRepository
            .Setup(x => x.GetUserByEmailAsync("inexistente@email.com"))
            .ReturnsAsync((User?)null);

        var handler = new GetUserByEmailHandler(mockRepository.Object, mockLogger.Object);
        var request = new GetUserByEmailRequest("inexistente@email.com");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => handler.Handle(request, CancellationToken.None)
        );

        exception.Message.Should().Contain("não encontrado");
    }
}