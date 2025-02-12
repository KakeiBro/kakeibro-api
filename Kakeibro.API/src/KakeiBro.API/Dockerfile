FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/KakeiBro.API/KakeiBro.API.csproj", "src/KakeiBro.API/"]
RUN dotnet restore "src/KakeiBro.API/KakeiBro.API.csproj"
COPY . .
WORKDIR "/src/src/KakeiBro.API"
RUN dotnet build "KakeiBro.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "KakeiBro.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KakeiBro.API.dll"]
