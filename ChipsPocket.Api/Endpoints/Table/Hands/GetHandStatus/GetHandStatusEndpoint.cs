using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Endpoints.Table.Hands.GetHandStatus;

public record ViewHandStatusDto(
    Guid HandId,
    int DealerSeatId,
    int SmallBlindSeatId,
    int BigBlindSeatId,
    Street CurrentStreet,
    Guid NextActionSeatId,
    int MinimumRaiseAmount,
    int PotValue);

public class GetHandStatusEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("", async Task<Results<
            NotFound<string>,
            ForbidHttpResult,
            BadRequest<string>,
            Ok<ViewHandStatusDto>>> (
            Guid tableId,
            IHandRepository handRepo,
            ITableRepository tableRepository,
            ICurrentUser currentUser) =>
        {
            throw new NotImplementedException();
        });
    }
}