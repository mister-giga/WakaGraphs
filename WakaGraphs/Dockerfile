#--#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#--
#--#FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
#--FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine AS base
#--WORKDIR /app
#--
#--#FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
#--FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS publish
#--WORKDIR /src
#--COPY ["DisplayWakaTimeStatsToGitHub.csproj", ""]
#--RUN dotnet restore "./DisplayWakaTimeStatsToGitHub.csproj" --runtime alpine-x64
#--COPY . .
#--WORKDIR "/src/."
#--#RUN dotnet build "DisplayWakaTimeStatsToGitHub.csproj" -c Release -o /app/build
#--
#--#RUN dotnet publish "DisplayWakaTimeStatsToGitHub.csproj" -r linux-x64 -c Release -o /app/publish
#--RUN dotnet publish "DisplayWakaTimeStatsToGitHub.csproj" -c Release --runtime alpine-x64 -o /app/publish --no-restore --self-contained true -p:PublishTrimmed=true -p:PublishSingleFile=true
#--
#--FROM base AS final
#--WORKDIR /app
#--COPY --from=publish /app/publish .
#--ENTRYPOINT ["dotnet", "/app/DisplayWakaTimeStatsToGitHub.dll"] #["pwd"]#["ls", "-R", "/"] #
#--
#--#dotnet publish "DisplayWakaTimeStatsToGitHub.csproj" -c Release -o /app/publish --self-contained true p:PublishTrimmed=true p:PublishSingleFile=true
#--
#--#dotnet publish -r linux-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true
#--
#--#dotnet publish -c Release --runtime alpine-x64 -o /app/publish --self-contained true -p:PublishTrimmed=true -p:PublishSingleFile=true
#--
#--
#--    ##71.8



FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS publish
WORKDIR /src
COPY ["DisplayWakaTimeStatsToGitHub.csproj", ""]

RUN dotnet restore "./DisplayWakaTimeStatsToGitHub.csproj" --runtime alpine-x64
COPY . .
RUN dotnet publish "DisplayWakaTimeStatsToGitHub.csproj" -c Release --runtime alpine-x64 -o /app/publish --no-restore --self-contained true -p:PublishTrimmed=true -p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/app/DisplayWakaTimeStatsToGitHub"]