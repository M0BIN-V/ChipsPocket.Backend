using ChipsPocket.Api.Infra.Persistence;
using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Endpoints.Table.GetTableInfo;

public record ViewUserDto(string Username);

public record ViewSeatDto(Guid Id, int Order, ViewUserDto? User);

public record ViewActiveHandDto(
    Guid TableId,
    Guid ArgId,
    Guid DealerSeatId,
    Guid SmallBlindSeatId,
    Guid BigBlindSeatId,
    Street CurrentStreet);

public record ViewTableInfoDto(
    Guid Id,
    string Name,
    IEnumerable<ViewSeatDto> Seats,
    string ManagerId,
    ViewActiveHandDto? ActiveHand);

public class GetTableInfoEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("{tableId:guid}", async Task<Results<
                ForbidHttpResult,
                Ok<ViewTableInfoDto>>> (
                [FromRoute] Guid tableId,
                [FromServices] ITableRepository tableRepository,
                [FromServices] AppDbContext db,
                [FromServices] ISeatRepository seatRepository,
                [FromServices] IMembersRepository membersRepository,
                [FromServices] ICurrentUser currentUser) =>
            {
                if (!await membersRepository.IsMemberOfTableAsync(tableId, currentUser.Id))
                    return Forbid();

                var table = await db.Tables
                    .Where(t => t.Id == tableId)
                    .Select(t => new ViewTableInfoDto(
                        t.Id,
                        t.Name,
                        t.Seats.Select(s => new ViewSeatDto(
                            s.Id,
                            s.Order,
                            s.User == null
                                ? null
                                : new ViewUserDto(s.User.UserName!))),
                        t.ManagerId,
                        t.Hands
                            .Where(h => h.CurrentStreet != Street.Finished)
                            .Select(h => new ViewActiveHandDto(
                                tableId,
                                h.Id,
                                h.DealerSeatId,
                                h.SmallBlindSeatId,
                                h.BigBlindSeatId,
                                h.CurrentStreet))
                            .SingleOrDefault()
                    ))
                    .SingleAsync();

                return Ok(table);
            })
            .WithSummary("Get table information")
            .WithDescription("""
                             Returns the details of a poker table, including its name and seats.

                             The authenticated user must be a member of the table.
                             Users who are not members of the table are forbidden from accessing
                             the table information.
                             """)
            .RequireAuthorization();
    }
}