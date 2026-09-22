using ChipsPocket.Api.Endpoints.Table.Lobby.GetJoinToken;
using ChipsPocket.Api.Endpoints.Table.Lobby.Join;

namespace ChipsPocket.Api.Endpoints.Table.Lobby;

public static class LobbyEndpointGroup
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapTablesEndpoints()
        {
            var group = endpoints
                .MapGroup("{tableId:guid}/lobby")
                .WithTags("Tables");

            GetJoinTokenEndpoint.Map(group);
            JoinToLobbyEndpoint.Map(group);

            return endpoints;
        }
    }
}