using ChipsPocket.Api.Endpoints.Table.BuyIn;
using ChipsPocket.Api.Endpoints.Table.ClaimSeat;
using ChipsPocket.Api.Endpoints.Table.CreateTable;
using ChipsPocket.Api.Endpoints.Table.GetMyTables;
using ChipsPocket.Api.Endpoints.Table.GetTableInfo;
using ChipsPocket.Api.Endpoints.Table.Lobby;
using ChipsPocket.Api.Endpoints.Table.ReleaseSeat;
using ChipsPocket.Api.Endpoints.Table.UserStack;

namespace ChipsPocket.Api.Endpoints.Table;

public static class TablesEndpointGroup
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapTablesEndpoints()
        {
            var group = endpoints
                .MapGroup("/api/tables")
                .WithTags("Tables")
                .WithDescription("Endpoints for creating and managing poker tables.");

            CreateTableEndpoint.Map(group);
            GetTableInfoEndpoint.Map(group);
            ClaimSeatEndpoint.Map(group);
            ReleaseSeatEndpoint.Map(group);
            BuyInEndpoint.Map(group);
            GetUserStackEndpoint.Map(group);
            GetMyTablesEndpoint.Map(group);

            group.MapLobbyEndpoints();

            return endpoints;
        }
    }
}