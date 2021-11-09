using APIGateway.Settings;
using Consul;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json");
// Add services to the container.

builder.Services.AddOcelot().AddConsul();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Consul

var consulClient = new ConsulClient(config =>
{
    config.Address = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()
        .ServiceDiscoveryAddress;
});
builder.Services.AddSingleton<IHostedService, ServiceDiscoveryHostedService>();
builder.Services.AddSingleton<IConsulClient, ConsulClient>(_ => consulClient);

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});
app.UseOcelot().Wait();


app.Run();