namespace Fcg.Shareable.Responses;

public record LoginResponse(
    bool Success,
    string Message,
    string? Token = null,
    string? Role = null
);
