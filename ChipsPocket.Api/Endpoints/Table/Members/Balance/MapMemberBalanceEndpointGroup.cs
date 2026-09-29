using ChipsPocket.Api.Endpoints.Table.Members.Balance.Add;
using ChipsPocket.Api.Endpoints.Table.Members.Balance.Deduct;
using ChipsPocket.Api.Endpoints.Table.Members.Balance.Get;

namespace ChipsPocket.Api.Endpoints.Table.Members.Balance;

public static class MapMemberBalanceEndpointGroup
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapMemberBalanceEndpoints()
        {
            var group = endpoints
                .MapGroup("{memberId}/balance")
                .WithTags("Balance")
                .WithDescription("Endpoints for managing members balance");

            AddBalanceToMemberEndpoint.Map(group);
            DeductMemberBalanceEndpoint.Map(group);
            GetMemberBalanceEndpoint.Map(group);

            return endpoints;
        }
    }
}