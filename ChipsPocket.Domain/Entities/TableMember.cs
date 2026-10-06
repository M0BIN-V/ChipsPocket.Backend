namespace ChipsPocket.Domain.Entities;

public class TableMember
{
    public string UserId { get; set; } = null!;

    public Guid TableId { get; set; }

    public int Stack { get; set; }
}