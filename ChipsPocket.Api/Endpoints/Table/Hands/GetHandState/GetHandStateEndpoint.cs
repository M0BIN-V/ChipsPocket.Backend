using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Endpoints.Table.Hands.GetHandState;

public class GetHandStateEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("", async Task<Results<
            NotFound<string>,
            ForbidHttpResult,
            BadRequest<string>,
            Ok<ActiveHand>>> (
            Guid tableId,
            IMembersRepository membersRepo,
            ICurrentUser currentUser,
            IActiveHandRepository activeHandRepo) =>
        {
            var userIsMember = await membersRepo.IsMemberOfTableAsync(tableId, currentUser.Id);
            if (!userIsMember) return Forbid();

            var hand = activeHandRepo.GetHand(tableId);

            if (hand is null) return NotFound("table not found");


            return Ok(hand);
        });
    }
}