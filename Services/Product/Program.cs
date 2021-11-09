using Consul;
using Product.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

app.MapControllers();

app.Run();