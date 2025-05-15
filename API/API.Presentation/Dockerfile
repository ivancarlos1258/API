# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API.G-Bov.Presentation/API.G-Bov.Presentation.csproj", "API.G-Bov.Presentation/"]
COPY ["API.G-Bov.Core/API.G-Bov.Domain.csproj", "API.G-Bov.Core/"]
COPY ["API.G-Bov.Utility/API.G-Bov.Utility.csproj", "API.G-Bov.Utility/"]
COPY ["API.G-Bov.Infra/API.G-Bov.Infra.csproj", "API.G-Bov.Infra/"]
COPY ["API.G-Bov.Service/API.G-Bov.Service.csproj", "API.G-Bov.Service/"]
RUN dotnet restore "./API.G-Bov.Presentation/API.G-Bov.Presentation.csproj"
COPY . .
WORKDIR "/src/API.G-Bov.Presentation"
RUN dotnet build "./API.G-Bov.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.G-Bov.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.G-Bov.Presentation.dll"]