using Fcg.Shareable.Responses;
using MediatR;

namespace Fcg.Shareable.Requests;

public record DeleteGameRequest(string name) : IRequest<DeleteGameResponse>;
