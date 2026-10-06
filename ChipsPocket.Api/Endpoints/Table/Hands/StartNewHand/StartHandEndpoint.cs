using ChipsPocket.Api.Infra.Persistence;
using ChipsPocket.Api.Notifications.Table;
using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;
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
            Ok<ActiveHand>>> (
            Guid tableId,
            IActiveHandRepository activeHandRepo,
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

            activeHandRepo.AddHand(hand);

            await publisher.PublishAsync(tableId, new HandStartedNotification(hand));

            actionManager.PostSmallBlind(hand);
            actionManager.PostBigBlind(hand);

            activeHandRepo.SaveHand(hand);
            await db.SaveChangesAsync();

            return Ok(hand);
        });
    }
}