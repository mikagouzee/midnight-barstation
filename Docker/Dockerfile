#FROM mcr.microsoft.com/dotnet/runtime-deps:9.0-noble-arm32v7 AS base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8069


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Barstation/Barstation.csproj Barstation/
RUN dotnet restore Barstation/Barstation.csproj
COPY . .  
WORKDIR "/src/Barstation"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Barstation.dll"]