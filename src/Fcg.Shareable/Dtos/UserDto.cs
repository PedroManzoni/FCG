namespace Fcg.Shareable.Dtos;

public record UserDto
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string? Password { get; set; }
}
