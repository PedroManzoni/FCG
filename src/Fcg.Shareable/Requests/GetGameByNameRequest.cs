using Fcg.Shareable.Responses;
using MediatR;

namespace Fcg.Shareable.Requests;

public record GetGameByNameRequest(string name) : IRequest<GetGameByNameResponse>;
