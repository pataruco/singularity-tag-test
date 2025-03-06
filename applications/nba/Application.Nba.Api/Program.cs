using Application.Nba.Api.Services.V1;

var builder = WebApplication.CreateBuilder(args);

// Add gRPC services
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "This is a gRPC service. Use a gRPC client to communicate.");

app.Run();