namespace ChipsPocket.Api.Realtime;

public sealed record RealtimeManifest(
    IReadOnlyList<RealtimeHubInfo> Hubs);

public sealed record RealtimeHubInfo(
    string Name,
    string Route,
    string? Description,
    IReadOnlyList<RealtimeMethodInfo> ClientMethods,
    IReadOnlyList<RealtimeEventInfo> ServerEvents);

public sealed record RealtimeMethodInfo(
    string Name,
    string? Description,
    IReadOnlyList<RealtimeParameterInfo> Parameters);

public sealed record RealtimeParameterInfo(
    string Name,
    string Type,
    string? Description);

public sealed record RealtimeEventInfo(
    string Name,
    string? Description,
    string PayloadType,
    object PayloadSchema);