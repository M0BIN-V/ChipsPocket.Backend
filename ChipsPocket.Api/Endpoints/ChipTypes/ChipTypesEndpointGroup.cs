using ChipsPocket.Api.Endpoints.ChipTypes.GetAll;

namespace ChipsPocket.Api.Endpoints.ChipTypes;

public static class ChipTypesEndpointGroup
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapChipsEndpoints()
        {
            var group = endpoints
                .MapGroup("/api/chip-types")
                .WithTags("chip types");

            GetAllChipTypesEndpoint.Map(group);

            return endpoints;
        }
    }
}