using ChipsPocket.Api.Common.Dtos;
using ChipsPocket.Api.Infra.Persistence;
using ChipsPocket.Api.Notifications.Table;
using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Services.HandActionManager;
using ChipsPocket.Domain.Services.HandManager;

namespace ChipsPocket.Api.Endpoints.Table.Hands.StartNewHand;

public class StartHandEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("", async Task<Results<
            NotFound<string>,
            ForbidHttpResult,
            BadRequest<string>,
            Ok<ViewCreatedHandDto>>> (
            Guid tableId,
            IHandRepository handRepo,
            IHandManagerService managerService,
            IHandActionManager actionManager,
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

            var viewCreatedHandDto = new ViewCreatedHandDto(
                hand.Id,
                hand.DealerSeatId,
                hand.SmallBlindSeatId,
                hand.BigBlindSeatId);

            await publisher.PublishAsync(tableId, new HandStartedNotification(viewCreatedHandDto));

            await actionManager.PostSmallBlindAsync(hand, table.SmallBlindAmount);
            await actionManager.PostBigBlindAsync(hand, table.BigBlindAmount);

            await db.SaveChangesAsync();

            return Ok(viewCreatedHandDto);
        });
    }
}