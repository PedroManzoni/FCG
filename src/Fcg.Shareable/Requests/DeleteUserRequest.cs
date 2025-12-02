using Fcg.Shareable.Responses;
using MediatR;

namespace Fcg.Shareable.Requests;

public record DeleteUserRequest(string email) : IRequest<DeleteUserResponse>;
