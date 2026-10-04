using ChipsPocket.Api.Infra.Persistence;
using ChipsPocket.Api.Infra.Persistence.EntityConfigs;
using ChipsPocket.Domain.Entities;

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
                [FromServices] ILogger<CreateTableEndpoint> logger,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                var userId = currentUser.Id;

                if (await db.Tables.AnyAsync(t => t.CreatedById == userId && t.Name == request.TableName))
                    return Conflict();

                var table = Domain.Entities.Table.Create(
                    request.TableName,
                    currentUser.Id,
                    request.SmallBlindAmount);

                table.ManagerId = userId;

                var seats = Enumerable.Range(1, 10)
                    .Select(order => new Seat
                    {
                        TableId = table.Id,
                        Order = order
                    })
                    .ToList();

                var member = new TableMember
                {
                    UserId = userId,
                    TableId = table.Id
                };

                await db.Tables.AddAsync(table);
                await db.TableMembers.AddAsync(member);
                await db.Seats.AddRangeAsync(seats);
                await db.SaveChangesAsync();

                logger.LogInformation("table created");

                return Created($"/api/tables/{table.Id}",
                    new CreateTableResponse(table.Id));
            })
            .WithSummary("Create a poker table")
            .WithDescription("""
                             Creates a new poker table for the authenticated user.

                             The authenticated user becomes the owner of the table and is
                             automatically added to the table's members.

                             A user cannot create multiple tables with the same name.
                             """)
            .Validate<CreateTableRequest>()
            .RequireAuthorization();
    }
}