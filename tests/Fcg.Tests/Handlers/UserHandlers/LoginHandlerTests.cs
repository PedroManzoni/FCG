using Fcg.Domain.UserHandlers;
using Microsoft.Extensions.Logging;

namespace Fcg.Tests.Handlers.UserHandlers;

public class LoginHandlerTests
{
    [Fact]
    public async Task Handle_DeveRetornarToken_QuandoCredenciaisValidas()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockTokenService = new Mock<ITokenService>();
        var mockLogger = new Mock<ILogger<LoginHandler>>();

        var senhaHash = BCrypt.Net.BCrypt.HashPassword("Senha@123");

        var usuario = new User
        {
            Id = Guid.NewGuid(),
            Name = "João Silva",
            Email = "joao@email.com",
            Password = senhaHash,
            Role = "User"
        };

        mockRepository
            .Setup(x => x.GetUserByEmailAsync("joao@email.com"))
            .ReturnsAsync(usuario);

        mockTokenService
            .Setup(x => x.GenerateToken(usuario))
            .Returns("token-jwt-gerado");

        var handler = new LoginHandler(mockRepository.Object, mockTokenService.Object, mockLogger.Object);

        var request = new LoginRequest(new LoginDto(
            Email: "joao@email.com",
            Password: "Senha@123"
        ));

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Login realizado com sucesso");
        response.Token.Should().Be("token-jwt-gerado");
        response.Role.Should().Be("User");

        mockTokenService.Verify(x => x.GenerateToken(usuario), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarUnauthorizedAccessException_QuandoEmailNaoExiste()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockTokenService = new Mock<ITokenService>();
        var mockLogger = new Mock<ILogger<LoginHandler>>();

        mockRepository
            .Setup(x => x.GetUserByEmailAsync("inexistente@email.com"))
            .ReturnsAsync((User?)null);

        var handler = new LoginHandler(mockRepository.Object, mockTokenService.Object, mockLogger.Object);

        var request = new LoginRequest(new LoginDto(
            Email: "inexistente@email.com",
            Password: "Senha@123"
        ));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => handler.Handle(request, CancellationToken.None)
        );

        exception.Message.Should().Contain("Email ou senha inválidos");
        mockTokenService.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DeveLancarUnauthorizedAccessException_QuandoSenhaIncorreta()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockTokenService = new Mock<ITokenService>();
        var mockLogger = new Mock<ILogger<LoginHandler>>();

        var senhaHash = BCrypt.Net.BCrypt.HashPassword("SenhaCorreta@123");

        var usuario = new User
        {
            Id = Guid.NewGuid(),
            Name = "João Silva",
            Email = "joao@email.com",
            Password = senhaHash,
            Role = "User"
        };

        mockRepository
            .Setup(x => x.GetUserByEmailAsync("joao@email.com"))
            .ReturnsAsync(usuario);

        var handler = new LoginHandler(mockRepository.Object, mockTokenService.Object, mockLogger.Object);

        var request = new LoginRequest(new LoginDto(
            Email: "joao@email.com",
            Password: "SenhaErrada@123"
        ));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => handler.Handle(request, CancellationToken.None)
        );

        exception.Message.Should().Contain("Email ou senha inválidos");
        mockTokenService.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
    }
}