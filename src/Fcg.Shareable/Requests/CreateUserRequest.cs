using Fcg.Shareable.Dtos;
using Fcg.Shareable.Responses;
using FluentValidation;
using MediatR;

namespace Fcg.Shareable.Requests;

public record CreateUserRequest(UserDto User) : IRequest<CreateUserResponse>;
