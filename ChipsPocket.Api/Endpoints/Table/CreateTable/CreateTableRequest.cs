namespace ChipsPocket.Api.Endpoints.Table.CreateTable;

public record CreateTableRequest(string TableName, int SmallBlindAmount);

public class CreateTableRequestValidator : AbstractValidator<CreateTableRequest>
{
    public CreateTableRequestValidator()
    {
        RuleFor(x => x.TableName)
            .MinimumLength(3)
            .MaximumLength(200);

        RuleFor(x => x.SmallBlindAmount)
            .GreaterThanOrEqualTo(0);
    }
}