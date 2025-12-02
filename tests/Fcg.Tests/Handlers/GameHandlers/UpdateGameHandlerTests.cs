using Fcg.Domain.GameHandlers;
using Microsoft.Extensions.Logging;

namespace Fcg.Tests.Handlers.GameHandlers;

public class UpdateGameHandlerTests
{
    [Fact]
    public async Task Handle_DeveAtualizarJogoComSucesso_QuandoJogoExiste()
    {
        // Arrange
        var mockRepository = new Mock<IGameRepository>();
        var mockLogger = new Mock<ILogger<UpdateGameHandler>>();

        var gameExistente = new Game
        {
            Id = Guid.NewGuid(),
            Name = "God of War",
            Description = "Descrição antiga",
            Price = 199.90m,
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            LastUpdatedAt = DateTime.UtcNow.AddDays(-10)
        };

        mockRepository
            .Setup(x => x.GetGameByNameAsync("God of War"))
            .ReturnsAsync(gameExistente);

        var handler = new UpdateGameHandler(mockRepository.Object, mockLogger.Object);

        var request = new UpdateGameRequest(
            "God of War",
            new GameDto
            {
                Name = "God of War Ragnarok",
                Description = "Nova descrição",
                Price = 249.90m
            }
        );

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.message.Should().Be("Jogo atualizado com sucesso");
        mockRepository.Verify(x => x.UpdateGameAsync(It.IsAny<Game>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarKeyNotFoundException_QuandoJogoNaoExiste()
    {
        // Arrange
        var mockRepository = new Mock<IGameRepository>();
        var mockLogger = new Mock<ILogger<UpdateGameHandler>>();

        mockRepository
            .Setup(x => x.GetGameByNameAsync("Jogo Inexistente"))
            .ReturnsAsync((Game?)null);

        var handler = new UpdateGameHandler(mockRepository.Object, mockLogger.Object);

        var request = new UpdateGameRequest(
            "Jogo Inexistente",
            new GameDto
            {
                Name = "Novo Nome",
                Description = "Nova descrição",
                Price = 100m
            }
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => handler.Handle(request, CancellationToken.None)
        );

        exception.Message.Should().Contain("não encontrado");
        mockRepository.Verify(x => x.UpdateGameAsync(It.IsAny<Game>()), Times.Never);
    }
}
