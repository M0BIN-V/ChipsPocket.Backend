using ChipsPocket.Api.Common.Extensions;
using ChipsPocket.Api.Infra.Persistence;
using ChipsPocket.Api.Notifications.Table;
using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;
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

                await publisher.PublishAsync(tableId, new HandStartedNotification(hand.ToDto()));

                activeHandRepo.SaveHand(hand);
                await db.SaveChangesAsync();

                return Ok(hand);
            })
            .WithDescription("""
                             Starts a new poker hand at the specified table.
                             Requirements:
                             - The authenticated user must be the table manager. 
                             - The table must be in a valid state to start a new hand. 
                             - The table must not already have an active hand. 
                             - At least two players must occupy seats. 
                             - Each player must have enough chips to cover the big blind.
                             """)
            .WithName("StartHand")
            .WithSummary("Start a new poker hand");
    }
}