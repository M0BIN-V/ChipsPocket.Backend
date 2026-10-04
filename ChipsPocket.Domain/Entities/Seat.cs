using ChipsPocket.Domain.Entities.Abstractions;

namespace ChipsPocket.Domain.Entities;

public class Seat : Entity
{
    public int Order { get; set; }
    public string? UserId { get; set; }
    public User? User { get; set; }
    public Guid TableId { get; set; }
}