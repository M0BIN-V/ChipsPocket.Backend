using ChipsPocket.Api.Abstractions.Endpionts;
using Microsoft.AspNetCore.Mvc;

namespace ChipsPocket.Api.Endpoints.ChipTypes.GetAll;

public record ViewChipTypeDto(Guid ChipId, string Name, string Picture);

public class GetAllChipTypesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("", async Task<Ok<List<ViewChipTypeDto>>> ([FromServices] AppDbContext db) =>
            {
                var chipAppearances = await db.Chips
                    .Select(appearance => new ViewChipTypeDto(appearance.Id, appearance.Name, appearance.Picture))
                    .ToListAsync();

                return TypedResults.Ok(chipAppearances);
            })
            .WithDescription("returns all chip types")
            .RequireAuthorization();
    }
}