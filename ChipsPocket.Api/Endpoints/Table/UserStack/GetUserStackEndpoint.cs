namespace ChipsPocket.Api.Endpoints.Table.UserStack;

public class GetUserStackEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("{tableId:guid}/user-stack/{userId}", async Task<Results<
                NotFound<string>,
                Ok<UserStackResponse>>> (
                [FromRoute] Guid tableId,
                [FromRoute] string userId,
                [FromServices] IUserStackService userStackService,
                CancellationToken cancellationToken) =>
            {
                var stack = await userStackService.GetAsync(
                    tableId,
                    userId,
                    cancellationToken);

                if (stack is null) return TypedResults.NotFound("Table not found.");

                return TypedResults.Ok(stack);
            })
            .WithName("GetUserStack")
            .WithTags("User Stack")
            .WithSummary("Get a user's chip stack")
            .WithDescription("""
                             Returns the current chip stack of a user at a specific table.

                             The stack is calculated from all chip transactions associated
                             with the specified user and table. Chips received by the user
                             are added to the stack, while chips sent by the user are removed.

                             Only chips with a positive balance are included in the response.
                             """)
            .RequireAuthorization();
    }
}