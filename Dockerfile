#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["ExpressYourself.API/ExpressYourself.API.csproj", "ExpressYourself.API/"]
COPY ["ExpressYourself.Application/ExpressYourself.Application.csproj", "ExpressYourself.Application/"]
COPY ["ExpressYourself.Domain/ExpressYourself.Domain.csproj", "ExpressYourself.Domain/"]
COPY ["ExpressYourself.Infrastructure/ExpressYourself.Infrastructure.csproj", "ExpressYourself.Infrastructure/"]
RUN dotnet restore "ExpressYourself.API/ExpressYourself.API.csproj"
COPY . .
WORKDIR "/src/ExpressYourself.API"
RUN dotnet build "./ExpressYourself.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./ExpressYourself.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER root

ENTRYPOINT ["dotnet", "ExpressYourself.API.dll"]