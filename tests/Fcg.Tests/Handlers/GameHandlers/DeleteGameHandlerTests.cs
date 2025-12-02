using Fcg.Domain.GameHandlers;
using Microsoft.Extensions.Logging;

namespace Fcg.Tests.Handlers.GameHandlers;

public class DeleteGameHandlerTests
{
    [Fact]
    public async Task Handle_DeveDeletarJogoComSucesso_QuandoJogoExiste()
    {
        // Arrange
        var mockRepository = new Mock<IGameRepository>();
        var mockLogger = new Mock<ILogger<DeleteGameHandler>>();

        var gameExistente = new Game
        {
            Id = Guid.NewGuid(),
            Name = "God of War",
            Description = "Jogo de ação",
            Price = 199.90m
        };

        mockRepository
            .Setup(x => x.GetGameByNameAsync("God of War"))
            .ReturnsAsync(gameExistente);

        var handler = new DeleteGameHandler(mockRepository.Object, mockLogger.Object);
        var request = new DeleteGameRequest("God of War");

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.message.Should().Be("Jogo deletado com sucesso");
        mockRepository.Verify(x => x.DeleteGameAsync("God of War"), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarKeyNotFoundException_QuandoJogoNaoExiste()
    {
        // Arrange
        var mockRepository = new Mock<IGameRepository>();
        var mockLogger = new Mock<ILogger<DeleteGameHandler>>();

        mockRepository
            .Setup(x => x.GetGameByNameAsync("Jogo Inexistente"))
            .ReturnsAsync((Game?)null);

        var handler = new DeleteGameHandler(mockRepository.Object, mockLogger.Object);
        var request = new DeleteGameRequest("Jogo Inexistente");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => handler.Handle(request, CancellationToken.None)
        );

        exception.Message.Should().Contain("não encontrado");
        mockRepository.Verify(x => x.DeleteGameAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DeveLancarException_QuandoNomeNull()
    {
        // Arrange
        var mockRepository = new Mock<IGameRepository>();
        var mockLogger = new Mock<ILogger<DeleteGameHandler>>();
        var handler = new DeleteGameHandler(mockRepository.Object, mockLogger.Object);
        var request = new DeleteGameRequest(null!);

        // Act & Assert
        await Assert.ThrowsAnyAsync<Exception>(
            () => handler.Handle(request, CancellationToken.None)
        );
    }
}