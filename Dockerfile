FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV DOCKERIZE_VERSION v0.7.0

# Instalando Dockerize
RUN apt-get update && apt-get install -y wget \
    && wget -O - https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz | tar xzf - -C /usr/local/bin \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MotoDispatch.Api/MotoDispatch.Api.csproj", "src/MotoDispatch.Api/"]
COPY ["src/MotoDispatch.Domain/MotoDispatch.Domain.csproj", "src/MotoDispatch.Domain/"]
COPY ["src/MotoDispatch.Infra.Storage/MotoDispatch.Infra.Storage.csproj", "src/MotoDispatch.Infra.Storage/"]
COPY ["src/MotoDispatch.Application/MotoDispatch.Application.csproj", "src/MotoDispatch.Application/"]
COPY ["src/MotoDispatch.Infra.Data.EF/MotoDispatch.Infra.Data.EF.csproj", "src/MotoDispatch.Infra.Data.EF/"]
RUN dotnet restore "src/MotoDispatch.Api/MotoDispatch.Api.csproj"
COPY . .
WORKDIR "/src/src/MotoDispatch.Api"
RUN dotnet build "MotoDispatch.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "MotoDispatch.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MotoDispatch.Api.dll"]