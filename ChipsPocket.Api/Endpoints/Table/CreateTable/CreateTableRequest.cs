namespace ChipsPocket.Api.Endpoints.Table.CreateTable;

public record CreateTableRequest(string TableName);

public class CreateTableRequestValidator : AbstractValidator<CreateTableRequest>
{
    public CreateTableRequestValidator()
    {
        RuleFor(x => x.TableName)
            .MinimumLength(3)
            .MaximumLength(200);
    }
}