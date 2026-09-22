using FluentValidation;

namespace ChipsPocket.Api.Endpoints.Table.CreateTable;

public record AddChipToTableDto(Guid ChipTypeId, int Value);

public record CreateTableRequest(string TableName, List<AddChipToTableDto> Chips);

public class CreateTableRequestValidator : AbstractValidator<CreateTableRequest>
{
    public CreateTableRequestValidator()
    {
        RuleFor(x => x.Chips)
            .Must(chips => chips.Select(x => x.ChipTypeId).Distinct().Count() == chips.Count)
            .WithMessage("Chip types must be unique.");

        RuleForEach(x => x.Chips)
            .SetValidator(new AddChipToTableDtoValidator());
    }
}

public class AddChipToTableDtoValidator : AbstractValidator<AddChipToTableDto>
{
    public AddChipToTableDtoValidator()
    {
        RuleFor(x => x.ChipTypeId)
            .NotEmpty();
    }
}