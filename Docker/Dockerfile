FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy-arm32v7 AS build
WORKDIR /app
RUN ls -l /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app
ENTRYPOINT ["dotnet", "Barstation.dll"]