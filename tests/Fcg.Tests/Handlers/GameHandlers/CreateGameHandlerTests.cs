using Fcg.Domain.GameHandlers;
using Microsoft.Extensions.Logging;

namespace Fcg.Tests.Handlers.GameHandlers;

public class CreateGameHandlerTests
{
    [Fact]
    public async Task Handle_DevecriarJogoComSucesso_QuandoNomeNaoExiste()
    {
        // Arrange
        var mockRepository = new Mock<IGameRepository>();
        var mockLogger = new Mock<ILogger<CreateGameHandler>>();

        mockRepository
            .Setup(x => x.GetGameByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Game?)null);

        var handler = new CreateGameHandler(mockRepository.Object, mockLogger.Object);


        var request = new CreateGameRequest(new GameDto
        {
            Name = "God of War",
            Description = "Jogo de ação",
            Price = 199.90m
        });

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.message.Should().Be("Jogo cadastrado com sucesso");

        mockRepository.Verify(x => x.CreateGameAsync(It.IsAny<Game>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarArgumentException_QuandoJogoJaExiste()
    {
        // Arrange
        var mockRepository = new Mock<IGameRepository>();
        var mockLogger = new Mock<ILogger<CreateGameHandler>>();

        var gameExistente = new Game
        {
            Id = Guid.NewGuid(),
            Name = "God of War",
            Description = "Já existe",
            Price = 199.90m
        };

        mockRepository
            .Setup(x => x.GetGameByNameAsync("God of War"))
            .ReturnsAsync(gameExistente);

        var handler = new CreateGameHandler(mockRepository.Object, mockLogger.Object);

        var request = new CreateGameRequest(new GameDto
        {
            Name = "God of War",
            Description = "Jogo de ação",
            Price = 199.90m
        });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => handler.Handle(request, CancellationToken.None)
        );

        exception.Message.Should().Contain("já está cadastrado");

        mockRepository.Verify(x => x.CreateGameAsync(It.IsAny<Game>()), Times.Never);
    }
}