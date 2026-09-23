namespace ChipsPocket.Api.Realtime;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
public sealed class RealtimeDescriptionAttribute(string description) : Attribute
{
    public string Description { get; } = description;
}