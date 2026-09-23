using System.Net;

namespace ChipsPocket.Api.EndpointFilters;

public class EndpointValidatorFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var inputData = context.GetArgument<T>(0);

        if (inputData is not null)
        {
            var validationResult = await validator.ValidateAsync(inputData);

            if (!validationResult.IsValid)
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: (int)HttpStatusCode.UnprocessableEntity);
        }

        return await next.Invoke(context);
    }
}

public static class ValidatorExtensions
{
    public static RouteHandlerBuilder Validate<T>(this RouteHandlerBuilder builder)
        where T : class
    {
        builder.ProducesValidationProblem();
        builder.AddEndpointFilter<EndpointValidatorFilter<T>>();

        return builder;
    }
}