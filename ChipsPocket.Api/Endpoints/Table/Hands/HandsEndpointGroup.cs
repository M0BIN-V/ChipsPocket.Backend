using ChipsPocket.Api.Endpoints.Table.Hands.StartNewHand;

namespace ChipsPocket.Api.Endpoints.Table.Hands;

public static class HandsEndpointGroup
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapHandsEndpoints()
        {
            var group = endpoints
                .MapGroup("{tableId:guid}/hands")
                .WithTags("hands")
                .WithDescription("Endpoints for creating and managing poker hands");

            StartHandEndpoint.Map(group);
            return endpoints;
        }
    }
}