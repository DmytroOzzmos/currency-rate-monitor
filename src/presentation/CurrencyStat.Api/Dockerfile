FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build
COPY . .
WORKDIR /build/src/presentation/CurrencyStat.Api

# Restore the NuGet packages
RUN dotnet restore --runtime linux-x64

# Build the project
RUN dotnet build --no-restore -c Release --runtime linux-x64 -p:Platform=x64

# Publish the project
RUN dotnet publish --no-restore --no-build -c Release -o /publish --runtime linux-x64 -p:Platform=x64

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "CurrencyStat.Api.dll"]