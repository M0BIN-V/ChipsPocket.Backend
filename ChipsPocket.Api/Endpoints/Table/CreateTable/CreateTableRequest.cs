namespace ChipsPocket.Api.Endpoints.Table.CreateTable;

public record CreateTableRequest(string TableName, int BigBlindAmount, int SmallBlindAmount);

public class CreateTableRequestValidator : AbstractValidator<CreateTableRequest>
{
    public CreateTableRequestValidator()
    {
        RuleFor(x => x.TableName)
            .MinimumLength(3)
            .MaximumLength(200);

        RuleFor(x => x.BigBlindAmount)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.SmallBlindAmount)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.SmallBlindAmount)
            .LessThanOrEqualTo(x => x.BigBlindAmount)
            .WithMessage("Small blind amount must be less than or equal to big blind amount.");

        RuleFor(x => x.BigBlindAmount)
            .GreaterThanOrEqualTo(x => x.SmallBlindAmount)
            .WithMessage("Big blind amount must be greater than or equal to small blind amount.");
    }
}