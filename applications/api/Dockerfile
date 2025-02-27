FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app
EXPOSE 5095

COPY ./dist/applications/api/Application.Api/net9.0 /app/

# Start app
ENTRYPOINT ["dotnet", "./Application.Api.dll", "--urls", "http://0.0.0.0:5095"]
