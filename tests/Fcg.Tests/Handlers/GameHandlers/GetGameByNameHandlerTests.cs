using Fcg.Domain.GameHandlers;
using Microsoft.Extensions.Logging;

namespace Fcg.Tests.Handlers.GameHandlers;

public class GetGameByNameHandlerTests
{
    [Fact]
    public async Task Handle_DeveRetornarJogo_QuandoJogoExiste()
    {
        // Arrange
        var mockRepository = new Mock<IGameRepository>();
        var mockLogger = new Mock<ILogger<GetGameByNameHandler>>();

        var gameExistente = new Game
        {
            Id = Guid.NewGuid(),
            Name = "God of War",
            Description = "Jogo de ação",
            Price = 199.90m,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        mockRepository
            .Setup(x => x.GetGameByNameAsync("God of War"))
            .ReturnsAsync(gameExistente);

        var handler = new GetGameByNameHandler(mockRepository.Object, mockLogger.Object);
        var request = new GetGameByNameRequest("God of War");

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.gameDto.Should().NotBeNull();
        response.gameDto.Name.Should().Be("God of War");
        response.gameDto.Description.Should().Be("Jogo de ação");
        response.gameDto.Price.Should().Be(199.90m);
    }

    [Fact]
    public async Task Handle_DeveLancarKeyNotFoundException_QuandoJogoNaoExiste()
    {
        // Arrange
        var mockRepository = new Mock<IGameRepository>();
        var mockLogger = new Mock<ILogger<GetGameByNameHandler>>();

        mockRepository
            .Setup(x => x.GetGameByNameAsync("Jogo Inexistente"))
            .ReturnsAsync((Game?)null);

        var handler = new GetGameByNameHandler(mockRepository.Object, mockLogger.Object);
        var request = new GetGameByNameRequest("Jogo Inexistente");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => handler.Handle(request, CancellationToken.None)
        );

        exception.Message.Should().Contain("não encontrado");
    }

    [Fact]
    public async Task Handle_DeveLancarException_QuandoNomeVazio()
    {
        // Arrange
        var mockRepository = new Mock<IGameRepository>();
        var mockLogger = new Mock<ILogger<GetGameByNameHandler>>();
        var handler = new GetGameByNameHandler(mockRepository.Object, mockLogger.Object);
        var request = new GetGameByNameRequest("");

        // Act & Assert
        await Assert.ThrowsAnyAsync<Exception>(
            () => handler.Handle(request, CancellationToken.None)
        );
    }
}