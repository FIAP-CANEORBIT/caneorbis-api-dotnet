FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["CaneOrbis.Api.csproj", "."]
RUN dotnet restore "CaneOrbis.Api.csproj"

COPY . .
RUN dotnet build "CaneOrbis.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CaneOrbis.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN adduser --system --group --no-create-home appuser
USER appuser

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CaneOrbis.Api.dll"]