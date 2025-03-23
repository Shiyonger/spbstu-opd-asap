FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SPbSTU.OPD.ASAP.Google/SPbSTU.OPD.ASAP.Google.csproj", "SPbSTU.OPD.ASAP.Google/"]
RUN dotnet restore "SPbSTU.OPD.ASAP.Google/SPbSTU.OPD.ASAP.Google.csproj"
COPY . .
WORKDIR "/src/SPbSTU.OPD.ASAP.Google"
RUN dotnet build "SPbSTU.OPD.ASAP.Google.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SPbSTU.OPD.ASAP.Google.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SPbSTU.OPD.ASAP.Google.dll"]
