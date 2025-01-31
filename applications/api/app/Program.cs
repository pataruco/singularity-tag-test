
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var message = "Hello World";

app.MapGet("/", () => message);

app.Run();
