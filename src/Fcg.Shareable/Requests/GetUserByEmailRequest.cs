using Fcg.Shareable.Responses;
using MediatR;

namespace Fcg.Shareable.Requests;

public record GetUserByEmailRequest(string email) : IRequest<GetUserByEmailResponse>;
