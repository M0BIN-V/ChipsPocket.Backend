namespace ChipsPocket.Api.Common.Dtos;

public class ViewHandSeatDto
{
    public required int Order { get; set; }
    public required ViewHandPlayerDto Player { get; set; }
    public required bool IsDealer { get; set; }
    public required bool IsSmallBlind { get; set; }
    public required bool IsBigBlind { get; set; }
    public required bool IsFolded { get; set; }
}