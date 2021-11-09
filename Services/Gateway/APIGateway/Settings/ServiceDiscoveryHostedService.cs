using Consul;

namespace APIGateway.Settings;

public class ServiceDiscoveryHostedService : IHostedService
{
    private string? _registrationId;
    private readonly IConsulClient _client;
    private readonly IConfiguration _configuration;

    public ServiceDiscoveryHostedService(IConsulClient client, IConfiguration configuration)
    {
        _client = client;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var serviceId = Environment.GetEnvironmentVariable("ServiceID") ?? Guid.NewGuid().ToString();
        var serviceName = _configuration.GetValue<string>("ServiceSettings:ServiceName");
        var serviceAddress = _configuration.GetValue<Uri>("ServiceSettings:ServiceAddress");

        _registrationId = $"{serviceName}-{serviceId}";
        Environment.SetEnvironmentVariable("ServiceID", serviceId);

        var registration = new AgentServiceRegistration
        {
            Name = serviceName,
            ID = _registrationId,
            Address = serviceAddress.Host,
            Port = serviceAddress.Port,
            Checks = new[]
            {
                new AgentServiceCheck
                {
                    Interval = TimeSpan.FromSeconds(5),
                    TCP = $"{serviceAddress.Host}:{serviceAddress.Port}",
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
                },
                new AgentServiceCheck
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    Interval = TimeSpan.FromSeconds(5),
                    HTTP =
                        $"http://{serviceAddress.Host}:{serviceAddress.Port}/HealthCheck?serviceId={Environment.GetEnvironmentVariable("ServiceID")}",
                }
            }
        };

        await _client.Agent.ServiceDeregister(registration.ID, cancellationToken);
        await _client.Agent.ServiceRegister(registration, cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.Agent.ServiceDeregister(_registrationId, cancellationToken);
    }
}