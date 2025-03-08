FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SPbSTU.OPD.ASAP.Core.Presentation/SPbSTU.OPD.ASAP.Core.Presentation.csproj", "SPbSTU.OPD.ASAP.Core.Presentation/"]
COPY ["SPbSTU.OPD.ASAP.Core.Infrastructure/SPbSTU.OPD.ASAP.Core.Infrastructure.csproj", "SPbSTU.OPD.ASAP.Core.Infrastructure/"]
COPY ["SPbSTU.OPD.ASAP.Core.Domain/SPbSTU.OPD.ASAP.Core.Domain.csproj", "SPbSTU.OPD.ASAP.Core.Domain/"]
RUN dotnet restore "SPbSTU.OPD.ASAP.Core.Presentation/SPbSTU.OPD.ASAP.Core.Presentation.csproj"
COPY . .
WORKDIR "/src/SPbSTU.OPD.ASAP.Core.Presentation"
RUN dotnet build "SPbSTU.OPD.ASAP.Core.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SPbSTU.OPD.ASAP.Core.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SPbSTU.OPD.ASAP.Core.Presentation.dll"]
