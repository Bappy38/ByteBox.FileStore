#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ByteBox.FileStore.API/ByteBox.FileStore.API.csproj", "src/ByteBox.FileStore.API/"]
COPY ["src/ByteBox.FileStore.Application/ByteBox.FileStore.Application.csproj", "src/ByteBox.FileStore.Application/"]
COPY ["src/ByteBox.FileStore.Infrastructure/ByteBox.FileStore.Infrastructure.csproj", "src/ByteBox.FileStore.Infrastructure/"]
COPY ["src/ByteBox.FileStore.Domain/ByteBox.FileStore.Domain.csproj", "src/ByteBox.FileStore.Domain/"]
RUN dotnet restore "./src/ByteBox.FileStore.API/ByteBox.FileStore.API.csproj"
COPY . .
WORKDIR "/src/src/ByteBox.FileStore.API"
RUN dotnet build "./ByteBox.FileStore.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ByteBox.FileStore.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ByteBox.FileStore.API.dll"]