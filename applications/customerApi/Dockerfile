FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
EXPOSE 5095

COPY ./dist/applications/customerApi/JustDirect.Applications.CustomerApi/net8.0 /app/

# Start app
ENTRYPOINT ["dotnet", "./JustDirect.Applications.CustomerApi.dll", "--urls", "http://0.0.0.0:5095"]
