using ChipsPocket.Api.Common.Dtos;
using ChipsPocket.Api.Common.Extensions;
using ChipsPocket.Domain.Contracts;

namespace ChipsPocket.Api.Endpoints.Table.Hands.GetActiveHandState;

public class GetActiveHandStateEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("active", async Task<Results<
                NotFound<string>,
                ForbidHttpResult,
                BadRequest<string>,
                Ok<ViewActiveHandDto>>> (
                Guid tableId,
                IMembersRepository membersRepo,
                ICurrentUser currentUser,
                IActiveHandRepository activeHandRepo) =>
            {
                var userIsMember = await membersRepo.IsMemberOfTableAsync(tableId, currentUser.Id);
                if (!userIsMember) return Forbid();

                var hand = activeHandRepo.GetHand(tableId);

                if (hand is null) return NotFound("active hand not found");


                return Ok(hand.ToDto());
            })
            .WithName("GetActiveHandState")
            .WithSummary("Get the active hand state");
    }
}