using ChipsPocket.Api.Endpoints.Table.CreateTable;

namespace ChipsPocket.Api.Endpoints.Table;

public static class TablesEndpointGroup
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapTablesEndpoints()
        {
            var group = endpoints
                .MapGroup("/api/tables")
                .WithTags("Tables");

            CreateTableEndpoint.Map(group);

            return endpoints;
        }
    }
}