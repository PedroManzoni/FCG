using Fcg.Shareable.Dtos;
using Fcg.Shareable.Responses;
using MediatR;

namespace Fcg.Shareable.Requests;

public record UpdateUserRequest(string email, UserDto user) : IRequest<UpdateUserResponse>;
