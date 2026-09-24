namespace ChipsPocket.Api.Endpoints.Chips.GetChips;

public record ViewChip(Guid ChipId, string Name, int Value, string Picture);

public class GetChipsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("chips", async Task<Ok<List<ViewChip>>> ([FromServices] AppDbContext db) =>
            {
                var chips = await db.Chips.OrderBy(c => c.Value)
                    .Select(c => new ViewChip(
                        c.Id,
                        c.Name,
                        c.Value,
                        c.Picture))
                    .ToListAsync();

                return TypedResults.Ok(chips);
            })
            .WithSummary("Get chips")
            .RequireAuthorization();
    }
}