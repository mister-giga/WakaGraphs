#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
#WORKDIR /app
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /src
#COPY ["WakaGraphs/WakaGraphs.csproj", "WakaGraphs/"]
#COPY ["WakaGraphs.Templates/WakaGraphs.Templates.csproj", "WakaGraphs.Templates/"]
#RUN dotnet restore "WakaGraphs/WakaGraphs.csproj"
#COPY . .
#WORKDIR "/src/WakaGraphs"
#RUN dotnet build "WakaGraphs.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "WakaGraphs.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "WakaGraphs.dll"]

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS publish
WORKDIR /src
COPY ["WakaGraphs/WakaGraphs.csproj", "./WakaGraphs/WakaGraphs.csproj"]

COPY . .
RUN dotnet restore "./WakaGraphs/WakaGraphs.csproj" --runtime alpine-x64
RUN dotnet publish "./WakaGraphs/WakaGraphs.csproj" -c Release --runtime alpine-x64 -o /app/publish --no-restore --self-contained true -p:PublishTrimmed=true -p:PublishSingleFile=true

#FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine AS final
FROM mcr.microsoft.com/dotnet/runtime:5.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/app/WakaGraphs"]