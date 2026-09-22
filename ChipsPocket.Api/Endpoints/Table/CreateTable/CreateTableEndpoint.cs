using ChipsPocket.Api.Abstractions.Endpionts;
using ChipsPocket.Api.EndpointFilters;
using Microsoft.AspNetCore.Mvc;

namespace ChipsPocket.Api.Endpoints.Table.CreateTable;

public class CreateTableEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("", async Task<Results<
                Created<CreateTableResponse>,
                Conflict,
                NotFound<string>>> (
                [FromBody] CreateTableRequest request,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                var userId = currentUser.Id;

                if (await db.Tables.AnyAsync(t => t.CreatedById == userId && t.Name == request.TableName))
                    return TypedResults.Conflict();

                var table = Data.Entities.Table.Create(request.TableName, currentUser.Id);


                await db.Tables.AddAsync(table);
                await db.SaveChangesAsync();

                return TypedResults.Created($"/api/tables/{table.Id}",
                    new CreateTableResponse(table.Id));
            })
            .Vaidate<CreateTableRequest>()
            .RequireAuthorization();
    }
}