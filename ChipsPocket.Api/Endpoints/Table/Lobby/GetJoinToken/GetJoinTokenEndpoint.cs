using ChipsPocket.Api.Abstractions.Endpionts;
using Microsoft.AspNetCore.Mvc;

namespace ChipsPocket.Api.Endpoints.Table.Lobby.GetJoinToken;

public record GetJoinTokenResponse(string Token);

public class GetJoinTokenEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("join-token", async Task<Results<
                Ok<GetJoinTokenResponse>,
                NotFound<string>>> (
                [FromRoute] Guid tableId,
                [FromServices] ITableJoinTokenService tokenService,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                var exists = await db.Tables
                    .AnyAsync(t => t.Id == tableId && t.CreatedById == currentUser.Id);

                if (!exists) return TypedResults.NotFound("table not found");

                var token = tokenService.Create(tableId);

                return TypedResults.Ok(new GetJoinTokenResponse(token));
            })
            .RequireAuthorization();
    }
}