using Adrec.Common;
using Adrec.Gateway.Config;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddAuth(builder.Configuration);


builder.Configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json")
                     .AddEnvironmentVariables();

builder.Services.AddOcelot().AddPolly();

builder.Services.AddSwaggerForOcelot(builder.Configuration);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

string[] methodsOrder = ["get", "post", "put", "patch", "delete", "options", "trace"];
builder.Services.AddSwaggerGen(c =>
{
    c.OrderActionsBy(apiDesc => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{Array.IndexOf(methodsOrder, apiDesc.HttpMethod?.ToLower())}");
});

var app = builder.Build();

app.UseCors();

app.UseSwagger();

app.UseWebSockets();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseSwaggerForOcelotUI(options =>
{
    options.DownstreamSwaggerEndPointBasePath = "/swagger/docs";
    options.ServerOcelot = "/";
    options.PathToSwaggerGenerator = "/swagger/docs";
    options.ReConfigureUpstreamSwaggerJson = AlterUpstream.AlterUpstreamSwaggerJson;
}).UseOcelot().Wait();

app.MapControllers();

await app.RunAsync();