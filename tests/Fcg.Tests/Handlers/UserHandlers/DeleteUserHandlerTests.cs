using Fcg.Domain.UserHandlers;
using Microsoft.Extensions.Logging;

namespace Fcg.Tests.Handlers.UserHandlers;

public class DeleteUserHandlerTests
{
    [Fact]
    public async Task Handle_DeveDeletarUsuarioComSucesso_QuandoUsuarioExiste()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<DeleteUserHandler>>();

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

        var handler = new DeleteUserHandler(mockRepository.Object, mockLogger.Object);
        var request = new DeleteUserRequest("joao@email.com");

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.message.Should().Be("Usuário deletado com sucesso");
        mockRepository.Verify(x => x.DeleteUserAsync("joao@email.com"), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarKeyNotFoundException_QuandoUsuarioNaoExiste()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<DeleteUserHandler>>();

        mockRepository
            .Setup(x => x.GetUserByEmailAsync("inexistente@email.com"))
            .ReturnsAsync((User?)null);

        var handler = new DeleteUserHandler(mockRepository.Object, mockLogger.Object);
        var request = new DeleteUserRequest("inexistente@email.com");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => handler.Handle(request, CancellationToken.None)
        );

        exception.Message.Should().Contain("não encontrado");
        mockRepository.Verify(x => x.DeleteUserAsync(It.IsAny<string>()), Times.Never);
    }
}