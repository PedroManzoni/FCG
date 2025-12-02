using Fcg.Shareable.Dtos;
using Fcg.Shareable.Responses;
using MediatR;

namespace Fcg.Shareable.Requests;

public record LoginRequest(LoginDto Login) : IRequest<LoginResponse>;
