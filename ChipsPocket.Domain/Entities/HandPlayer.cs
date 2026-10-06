namespace ChipsPocket.Domain.Entities;

public class HandPlayer
{
    public required string UserId { get; set; }
    public required string Username { get; set; }
    public required int Stack { get; set; }
}