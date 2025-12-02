using Fcg.Shareable.Dtos;
using Fcg.Shareable.Responses;
using MediatR;

namespace Fcg.Shareable.Requests;

public record CreateGameRequest(GameDto Game) : IRequest<CreateGameResponse>;
