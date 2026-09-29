using ChipsPocket.Api.Endpoints.Table.Members.Balance;
using ChipsPocket.Api.Endpoints.Table.Members.GetJoinToken;
using ChipsPocket.Api.Endpoints.Table.Members.GetMembers;
using ChipsPocket.Api.Endpoints.Table.Members.Join;
using ChipsPocket.Api.Endpoints.Table.Members.Remove;

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

            var groupWithTableId = endpoints.MapGroup("{tableId:guid}/members");

            JoinToTableEndpoint.Map(group);
            GetJoinTokenEndpoint.Map(groupWithTableId);
            GetMembersEndpoint.Map(groupWithTableId);
            RemoveFromMembersEndpoint.Map(groupWithTableId);

            groupWithTableId.MapMemberBalanceEndpoints();

            return endpoints;
        }
    }
}