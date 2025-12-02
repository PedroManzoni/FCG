using Fcg.Api.Filters;
using Fcg.Shareable.Dtos;
using Fcg.Shareable.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fcg.Api;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var LoginGroupBuilder = app.MapGroup("/api/v1/auth").WithTags("Authentication");

        LoginGroupBuilder.MapPost("/login", static async ([FromBody] LoginDto login, IMediator mediator)
            => await mediator.Send(new LoginRequest(login)))
            .AllowAnonymous()
            .WithName("Login");

        var UserGroupBuilder = app.MapGroup("/api/v1/user").WithTags("User");

        UserGroupBuilder.MapPost("/create", static async ([FromBody] UserDto user, IMediator mediator)
            => await mediator.Send(new CreateUserRequest(user)))
            .AddEndpointFilter<ValidationFilter<UserDto>>()
            .AllowAnonymous()
            .WithName("CreateUser");

        UserGroupBuilder.MapGet("/get/{email}", static async (IMediator mediator, [FromRoute] string email)
            => await mediator.Send(new GetUserByEmailRequest(email)))
            .RequireAuthorization()
            .WithName("GetUserById");

        UserGroupBuilder.MapPut("/update/{email}", static async (IMediator mediator, [FromRoute] string email, [FromBody] UserDto user)
            => await mediator.Send(new UpdateUserRequest(email, user)))
            .AddEndpointFilter<ValidationFilter<UserDto>>()
            .RequireAuthorization()
            .WithName("UpdateUser");

        UserGroupBuilder.MapDelete("/delete/{email}", static async (IMediator mediator, [FromRoute] string email)
            => await mediator.Send(new DeleteUserRequest(email)))
            .RequireAuthorization("Admin")
            .WithName("DeleteUser");

        var GameGroupBuilder = app.MapGroup("/api/v1/game").WithTags("Game");

        GameGroupBuilder.MapPost("/create", static async ([FromBody] GameDto game, IMediator mediator)
            => await mediator.Send(new CreateGameRequest(game)))
            .AddEndpointFilter<ValidationFilter<GameDto>>()
            .RequireAuthorization("Admin") 
            .WithName("CreateGame");

        GameGroupBuilder.MapGet("/get/{name}", static async (IMediator mediator, [FromRoute] string name)
            => await mediator.Send(new GetGameByNameRequest(name)))
            .AllowAnonymous()
            .WithName("GetGameByName");

        GameGroupBuilder.MapPut("/update/{name}", static async (IMediator mediator, [FromRoute] string name, [FromBody] GameDto game)
            => await mediator.Send(new UpdateGameRequest(name, game)))
            .AddEndpointFilter<ValidationFilter<GameDto>>()
            .RequireAuthorization("Admin")
            .WithName("UpdateGame");

        GameGroupBuilder.MapDelete("/delete/{name}", static async (IMediator mediator, [FromRoute] string name)
            => await mediator.Send(new DeleteGameRequest(name)))
            .RequireAuthorization("Admin")
            .WithName("DeleteGame");
    }
}