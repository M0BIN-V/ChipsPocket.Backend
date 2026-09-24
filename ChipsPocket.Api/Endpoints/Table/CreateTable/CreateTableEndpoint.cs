namespace ChipsPocket.Api.Endpoints.Table.CreateTable;

public class CreateTableEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("", async Task<Results<
                Created<CreateTableResponse>,
                Conflict,
                NotFound<string>>> (
                [FromBody] CreateTableRequest request,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                var userId = currentUser.Id;

                if (await db.Tables.AnyAsync(t => t.CreatedById == userId && t.Name == request.TableName))
                    return TypedResults.Conflict();

                var table = Data.Entities.Table.Create(
                    request.TableName,
                    currentUser.Id,
                    request.SmallBlindAmount,
                    request.BigBlindAmount);

                table.Members.Add(new TableMember
                {
                    UserId = userId,
                    TableId = table.Id
                });

                table.ManagerId = userId;

                await db.Tables.AddAsync(table);
                await db.SaveChangesAsync();

                return TypedResults.Created($"/api/tables/{table.Id}",
                    new CreateTableResponse(table.Id));
            })
            .WithSummary("Create a poker table")
            .WithDescription("""
                             Creates a new poker table for the authenticated user.

                             The authenticated user becomes the owner of the table and is
                             automatically added to the table's lobby.

                             A user cannot create multiple tables with the same name.
                             """)
            .Validate<CreateTableRequest>()
            .RequireAuthorization();
    }
}