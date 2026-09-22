using ChipsPocket.Api.Endpoints.Table.CreateTable;
using ChipsPocket.Api.Endpoints.Table.GetTableInfo;

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

            return endpoints;
        }
    }
}