using System.Reflection;
using Microsoft.AspNetCore.SignalR;

namespace ChipsPocket.Api.Realtime;

public static class RealtimeExtensions
{
    public static IServiceCollection AddRealtime(
        this IServiceCollection services)
    {
        services.AddSignalR();

        services.AddSingleton<RealtimeRegistry>();

        return services;
    }

    public static IEndpointRouteBuilder MapRealtimeHub<THub>(
        this IEndpointRouteBuilder endpoints,
        string route,
        string? description = null)
        where THub : Hub
    {
        endpoints.MapHub<THub>(route);

        var registry = endpoints.ServiceProvider
            .GetRequiredService<RealtimeRegistry>();

        registry.AddHub(
            typeof(THub),
            route,
            description);

        return endpoints;
    }

    public static RealtimeRegistry RegisterRealtimeEvent<TPayload>(
        this RealtimeRegistry registry,
        Type hubType,
        string name,
        string? description = null)
    {
        registry.AddEvent<TPayload>(
            hubType,
            description);

        return registry;
    }

    public static IEndpointRouteBuilder MapRealtimeManifest(
        this IEndpointRouteBuilder endpoints,
        string route = "/api/realtime")
    {
        endpoints.MapGet(route, (
                RealtimeRegistry registry) =>
            {
                var manifest = CreateManifest(registry);

                return Results.Ok(manifest);
            })
            .WithName("RealtimeManifest")
            .WithTags("Realtime");

        return endpoints;
    }

    private static RealtimeManifest CreateManifest(
        RealtimeRegistry registry)
    {
        var hubs = registry.Hubs
            .Select(hub => CreateHubInfo(hub, registry))
            .ToList();

        return new RealtimeManifest(hubs);
    }

    private static RealtimeHubInfo CreateHubInfo(
        RealtimeHubRegistration registration,
        RealtimeRegistry registry)
    {
        var methods = registration.HubType
            .GetMethods(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.DeclaredOnly)
            .Where(IsClientMethod)
            .Select(CreateMethodInfo)
            .ToList();

        var events = registry.Events
            .Where(x => x.HubType == registration.HubType)
            .Select(CreateEventInfo)
            .ToList();

        return new RealtimeHubInfo(
            registration.HubType.Name,
            registration.Route,
            registration.Description,
            methods,
            events);
    }

    private static bool IsClientMethod(MethodInfo method)
    {
        if (method.IsSpecialName)
            return false;

        if (method.IsStatic)
            return false;

        if (method.ContainsGenericParameters)
            return false;

        return method.ReturnType == typeof(Task) ||
               method.ReturnType == typeof(ValueTask) ||
               (method.ReturnType.IsGenericType &&
                (method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>) ||
                 method.ReturnType.GetGenericTypeDefinition() == typeof(ValueTask<>)));
    }

    private static RealtimeMethodInfo CreateMethodInfo(
        MethodInfo method)
    {
        var description = method
            .GetCustomAttribute<RealtimeDescriptionAttribute>()
            ?.Description;

        var parameters = method
            .GetParameters()
            .Select(parameter =>
            {
                var parameterDescription = parameter
                    .GetCustomAttribute<RealtimeDescriptionAttribute>()
                    ?.Description;

                return new RealtimeParameterInfo(
                    parameter.Name ?? "unknown",
                    GetTypeName(parameter.ParameterType),
                    parameterDescription);
            })
            .ToList();

        return new RealtimeMethodInfo(
            method.Name,
            description,
            parameters);
    }

    private static RealtimeEventInfo CreateEventInfo(
        RealtimeEventRegistration registration)
    {
        return new RealtimeEventInfo(
            registration.Name,
            registration.Description,
            registration.PayloadType.Name,
            RealtimeSchemaGenerator.Generate(
                registration.PayloadType));
    }

    private static string GetTypeName(Type type)
    {
        if (Nullable.GetUnderlyingType(type) is { } nullable)
            return $"{GetTypeName(nullable)}?";

        if (!type.IsGenericType)
            return type.Name;

        var name = type.Name[..type.Name.IndexOf('`')];

        var arguments = string.Join(
            ", ",
            type.GetGenericArguments()
                .Select(GetTypeName));

        return $"{name}<{arguments}>";
    }
}