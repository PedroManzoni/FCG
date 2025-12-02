using FluentValidation;

namespace Fcg.Api.Filters;

public class ValidationFilter<TRequest> : IEndpointFilter
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationFilter(IValidator<TRequest>? validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        if (_validator is null)
            return await next(context);

        var request = context.Arguments.OfType<TRequest>().FirstOrDefault();

        if (request is null)
            return await next(context);

        var validationResult = await _validator.ValidateAsync((TRequest)request);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                ));
        }

        return await next(context);
    }
}
