using Libraries.DotnetLib.DotnetLib;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var message = new HelloWorld().Greet();

app.MapGet("/", () => message);

app.Run();
