# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ProductManagementApi.csproj", "./"]
RUN dotnet restore "ProductManagementApi.csproj"
COPY . .
RUN dotnet publish "ProductManagementApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final or Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8081
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductManagementApi.dll"]

