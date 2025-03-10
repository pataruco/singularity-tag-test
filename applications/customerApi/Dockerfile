FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app
EXPOSE 5095

COPY ./dist/applications/api/JustDirect.Applications.CustomerApi/net9.0 /app/

# Start app
ENTRYPOINT ["dotnet", "./JustDirect.Applications.CustomerApi.dll", "--urls", "http://0.0.0.0:5095"]
