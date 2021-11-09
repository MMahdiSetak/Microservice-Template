namespace APIGateway.Settings;

public class ServiceSettings
{
    public Uri ServiceAddress { get; init; } = null!;
    public string ServiceName { get; init; } = null!;
    public Uri ServiceDiscoveryAddress { get; init; } = null!;
}