# Base image to host the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /Todo
EXPOSE 8080

# Build stage for compiling and publishing the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /Todo/src

COPY . .
RUN dotnet restore
RUN dotnet build -c $BUILD_CONFIGURATION -o /Todo/build

# Publish stage - create the final publish directory
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish -c $BUILD_CONFIGURATION -o /Todo/publish /p:UseAppHost=false

# Final stage with the runtime environment
FROM base AS final
WORKDIR /Todo
COPY --from=publish /Todo/publish .
ENTRYPOINT ["dotnet", "Todo.Api.dll"]
