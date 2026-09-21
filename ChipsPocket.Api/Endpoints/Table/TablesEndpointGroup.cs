using ChipsPocket.Api.EndpointFilters;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ChipsPocket.Api.Endpoints.Table;

public record CreateTableRequest(string TableName, int SeatCount);

public class CreateTableRequestValidator : AbstractValidator<CreateTableRequest>
{
    public CreateTableRequestValidator()
    {
        RuleFor(x => x.SeatCount)
            .GreaterThanOrEqualTo(2)
            .LessThanOrEqualTo(10);
    }
}

public static class TablesEndpointGroup
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapTablesEndpoints()
        {
            var group = endpoints
                .MapGroup("/api/tables")
                .WithTags("Tables");

            group.MapCreate();

            return endpoints;
        }

        private IEndpointRouteBuilder MapCreate()
        {
            endpoints.MapPost("", async Task<Results<Created<TableCreatedResponse>, Conflict>> (
                    [FromBody] CreateTableRequest request,
                    [FromServices] AppDbContext db,
                    [FromServices] ICurrentUser currentUser) =>
                {
                    var userId = currentUser.Id;

                    if (await db.Tables.AnyAsync(t => t.CreatedById == userId && t.Name == request.TableName))
                        return TypedResults.Conflict();

                    var seats = Enumerable.Range(0, request.SeatCount)
                        .Select(seatIndex => new Seat { Order = seatIndex + 1 })
                        .ToList();

                    var table = new Data.Entities.Table
                    {
                        CreatedById = userId,
                        Name = request.TableName,
                        CreatedAt = DateTimeOffset.UtcNow,
                        Seats = seats
                    };

                    await db.Tables.AddAsync(table);
                    await db.SaveChangesAsync();

                    return TypedResults.Created($"/api/tables/{table.Id}", new TableCreatedResponse(table.Id));
                })
                .Vaidate<CreateTableRequest>()
                .RequireAuthorization();

            return endpoints;
        }
    }

    public record TableCreatedResponse(Guid Id);
}