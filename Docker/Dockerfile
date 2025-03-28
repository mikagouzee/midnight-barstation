# Step 1: Choose a lightweight base image suitable for ARM architecture
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim-arm32v7 AS base
WORKDIR /app

# Step 2: Use a .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim-arm32v7 AS build
WORKDIR /src
RUN pwd
COPY ["./Barstation/Barstation.csproj", "./"]
RUN dotnet restore "Barstation.csproj" --verbosity detailed
COPY . .
RUN dotnet publish -c Release -o /app

# Step 3: Copy the build output to the runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Barstation.dll"]