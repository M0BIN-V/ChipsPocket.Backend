using ChipsPocket.Api.Endpoints.Chips.GetChips;

namespace ChipsPocket.Api.Endpoints.Chips;

public static class ChipsEndpointGroup
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapChipsEndpoints()
        {
            var group = endpoints
                .MapGroup("/api/chips")
                .WithTags("Chips")
                .WithDescription("Endpoints managing poker chips.");


            GetChipsEndpoint.Map(group);

            return endpoints;
        }
    }
}