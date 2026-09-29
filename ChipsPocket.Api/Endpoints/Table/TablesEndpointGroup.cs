using ChipsPocket.Api.Endpoints.Table.ClaimSeat;
using ChipsPocket.Api.Endpoints.Table.CreateTable;
using ChipsPocket.Api.Endpoints.Table.GetMyTables;
using ChipsPocket.Api.Endpoints.Table.GetTableInfo;
using ChipsPocket.Api.Endpoints.Table.Hands;
using ChipsPocket.Api.Endpoints.Table.Members;
using ChipsPocket.Api.Endpoints.Table.ReleaseSeat;

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
            GetMyTablesEndpoint.Map(group);

            group.MapMembersEndpoints();
            group.MapHandsEndpoints();

            return endpoints;
        }
    }
}