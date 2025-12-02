using Fcg.Shareable.Dtos;
using Fcg.Shareable.Responses;
using MediatR;

namespace Fcg.Shareable.Requests;

public record UpdateGameRequest(string name, GameDto updatedGame) : IRequest<UpdateGameResponse>;
