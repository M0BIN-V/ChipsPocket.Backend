using ChipsPocket.Api.Infra.Persistence;
using ChipsPocket.Api.Notifications.Table;
using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Services.HandManager;

namespace ChipsPocket.Api.Endpoints.Table.Hands.StartNewHand;

public record ViewCreateHandDto(
    Guid HandId,
    Guid DealerSeatId,
    Guid BigBlindSeatId,
    Guid SmallBlindSeatId);

public class StartHandEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("", async Task<Results<
            NotFound<string>,
            ForbidHttpResult,
            BadRequest<string>,
            Ok<ViewCreateHandDto>>> (
            Guid tableId,
            IHandRepository handRepo,
            IHandManagerService managerService,
            ITableRepository tableRepository,
            ITableNotificationPublisher publisher,
            ICurrentUser currentUser,
            AppDbContext db) =>
        {
            //validate table
            var table = await tableRepository.GetTableAsync(tableId);
            if (table is null) return NotFound("Table not found");

            //validate managerService
            if (table.ManagerId != currentUser.Id) return Forbid();

            //validate table 
            var (tableIsValid, errorMessage) = await managerService.TableIsValidToStartHandAsync(tableId);
            if (!tableIsValid) return BadRequest(errorMessage);

            var hand = await managerService.SetupHandAsync(tableId);

            handRepo.AddHand(hand);

            await db.SaveChangesAsync();

            var response = new ViewCreateHandDto(
                hand.Id,
                hand.DealerSeatId,
                hand.SmallBlindSeatId,
                hand.BigBlindSeatId);

            await publisher.PublishAsync(tableId, new HandStartedNotification(hand.Id));

            return Ok(response);
        });
    }
}