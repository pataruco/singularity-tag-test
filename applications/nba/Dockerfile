FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app
EXPOSE 5053

COPY ./dist/applications/nba/Application.Nba.Api/net9.0 /app/

# Start app
ENTRYPOINT ["dotnet", "./Application.Nba.Api.dll", "--urls", "http://0.0.0.0:5053"]
