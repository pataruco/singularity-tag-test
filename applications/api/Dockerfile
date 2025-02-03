# Ensure all dependencies have been resolved 
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS publish
COPY . .
RUN dotnet restore "./Application.Api/Application.Api.csproj"
RUN dotnet publish "./Application.Api/Application.Api.csproj" -c Release -o /app/publish --no-restore

# Copy app executable into new image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 5095
COPY --from=publish /app/publish .

# Start app
ENTRYPOINT ["dotnet", "Application.Api.dll", "--urls", "http://0.0.0.0:5095"]