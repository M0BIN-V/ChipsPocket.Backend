using ChipsPocket.Api.Endpoints.Table.Members.GetMembers;
using ChipsPocket.Api.Endpoints.Table.Members.Remove;
using ChipsPocket.Api.Endpoints.Table.Players.GetJoinToken;
using ChipsPocket.Api.Endpoints.Table.Players.Join;

namespace ChipsPocket.Api.Endpoints.Table.Members;

public static class MembersEndpointGroup
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapMembersEndpoints()
        {
            var group = endpoints
                .MapGroup("members")
                .WithTags("Tables")
                .WithDescription("Endpoints for managing and joining a table members.");

            GetJoinTokenEndpoint.Map(group);
            JoinToTableEndpoint.Map(group);
            GetMembersEndpoint.Map(group);
            RemoveFromMembersEndpoint.Map(group);

            return endpoints;
        }
    }
}