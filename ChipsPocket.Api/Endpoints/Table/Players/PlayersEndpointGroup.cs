using ChipsPocket.Api.Endpoints.Table.Lobby.LeftTable;
using ChipsPocket.Api.Endpoints.Table.Players.GetJoinToken;
using ChipsPocket.Api.Endpoints.Table.Players.GetPlayers;
using ChipsPocket.Api.Endpoints.Table.Players.Join;

namespace ChipsPocket.Api.Endpoints.Table.Players;

public static class PlayersEndpointGroup
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapPlayersEndpoints()
        {
            var group = endpoints
                .MapGroup("players")
                .WithTags("Tables")
                .WithDescription("Endpoints for managing and joining a table players.");

            GetJoinTokenEndpoint.Map(group);
            JoinToTableEndpoint.Map(group);
            GetPlayersEndpoint.Map(group);
            RemoveFromPlayersEndpoint.Map(group);

            return endpoints;
        }
    }
}