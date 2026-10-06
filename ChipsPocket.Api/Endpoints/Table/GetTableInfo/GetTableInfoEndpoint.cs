using ChipsPocket.Domain.Contracts;

namespace ChipsPocket.Api.Endpoints.Table.GetTableInfo;

public record ViewUserDto(string Username);

public record ViewSeatDto(Guid Id, int Order, ViewUserDto? User);

public record ViewTableInfoDto(
    Guid Id,
    string Name,
    IEnumerable<ViewSeatDto> Seats,
    string ManagerId,
    bool HasActiveHand);

public class GetTableInfoEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("{tableId:guid}", async Task<Results<
                ForbidHttpResult,
                Ok<ViewTableInfoDto>>> (
                Guid tableId,
                // AppDbContext db,
                ITableRepository tableRepo,
                IActiveHandRepository activeHandRepo,
                IMembersRepository membersRepository,
                ISeatRepository seatRepo,
                ICurrentUser currentUser) =>
            {
                if (!await membersRepository.IsMemberOfTableAsync(tableId, currentUser.Id))
                    return Forbid();

                var tableHasActiveHand = activeHandRepo.GetHand(tableId) is not null;

                var table = await tableRepo.GetTableAsync(tableId);
                if (table is null) return Forbid();

                var tableSeats = await seatRepo.GetSeatsAsync(tableId);

                var tableDto = new ViewTableInfoDto(
                    table.Id,
                    table.Name,
                    tableSeats.Select(s => new ViewSeatDto(
                        s.Id,
                        s.Order,
                        s.User == null
                            ? null
                            : new ViewUserDto(s.User.UserName!))),
                    table.ManagerId,
                    tableHasActiveHand
                );


                return Ok(tableDto);
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