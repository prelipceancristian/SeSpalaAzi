# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /SeSpalaAzi3
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /SeSpalaAzi3
COPY --from=build /SeSpalaAzi3/out .
ENTRYPOINT ["dotnet", "SeSpalaAzi3.dll"]