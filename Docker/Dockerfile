FROM mcr.microsoft.com/dotnet/nightly/runtime:9.0-noble AS base
WORKDIR /app
EXPOSE 8069

FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0-noble AS build
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