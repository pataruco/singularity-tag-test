FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app
EXPOSE 5053

COPY ./dist/applications/nba/JustDirect.Applications.Nba.Api/net8.0 /app/

# Start app
ENTRYPOINT ["dotnet", "./JustDirect.Applications.Nba.Api.dll", "--urls", "http://0.0.0.0:5053"]
