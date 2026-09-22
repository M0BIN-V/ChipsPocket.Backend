using ChipsPocket.Api.Endpoints.Table.Lobby.GetJoinToken;
using ChipsPocket.Api.Endpoints.Table.Lobby.Join;

namespace ChipsPocket.Api.Endpoints.Table.Lobby;

public static class LobbyEndpointGroup
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapLobbyEndpoints()
        {
            var group = endpoints
                .MapGroup("lobby")
                .WithTags("Tables")
                .WithDescription("""
                                 Endpoints for managing and joining a table lobby.

                                 A lobby represents the waiting area for users who want to
                                 participate in a poker table.
                                 """);

            GetJoinTokenEndpoint.Map(group);
            JoinToLobbyEndpoint.Map(group);

            return endpoints;
        }
    }
}