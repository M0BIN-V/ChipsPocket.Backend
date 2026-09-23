using Microsoft.AspNetCore.SignalR;

namespace ChipsPocket.Api.Realtime;

public sealed class RealtimeRegistry
{
    private readonly List<RealtimeEventRegistration> _events = [];
    private readonly List<RealtimeHubRegistration> _hubs = [];

    public IReadOnlyList<RealtimeHubRegistration> Hubs => _hubs;

    public IReadOnlyList<RealtimeEventRegistration> Events => _events;

    public void AddHub(
        Type hubType,
        string route,
        string? description = null)
    {
        if (!typeof(Hub).IsAssignableFrom(hubType))
            throw new ArgumentException(
                $"{hubType.Name} must inherit from Hub.",
                nameof(hubType));

        if (_hubs.Any(x => x.HubType == hubType))
            throw new InvalidOperationException(
                $"Hub '{hubType.Name}' is already registered.");

        _hubs.Add(new RealtimeHubRegistration(
            hubType,
            route,
            description));
    }

    public void AddEvent<TPayload>(
        Type hubType,
        string? description = null)
    {
        var name = typeof(TPayload).Name;

        if (_events.Any(x =>
                x.Name == name &&
                x.HubType == hubType))
            throw new InvalidOperationException(
                $"Realtime event '{name}' is already registered for hub '{hubType.Name}'.");

        _events.Add(new RealtimeEventRegistration(
            name,
            typeof(TPayload),
            description,
            hubType));
    }
}

public sealed record RealtimeHubRegistration(
    Type HubType,
    string Route,
    string? Description);

public sealed record RealtimeEventRegistration(
    string Name,
    Type PayloadType,
    string? Description,
    Type HubType);