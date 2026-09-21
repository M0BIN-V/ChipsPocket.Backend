namespace ChipsPocket.Api.Abstractions.Endpionts;

public interface IEndpoint
{
    static abstract void Map(IEndpointRouteBuilder group);
}