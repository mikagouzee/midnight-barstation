FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy-arm32v7 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy-arm32v7 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Barstation/Barstation.csproj", "Barstation/"]
RUN dotnet restore "Barstation/Barstation.csproj"
COPY . .
WORKDIR "/src/Barstation"
RUN dotnet build "Barstation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Barstation.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Barstation.dll"]