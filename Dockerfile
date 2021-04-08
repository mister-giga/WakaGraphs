FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS publish
WORKDIR /src
COPY ["WakaGraphs/WakaGraphs.csproj", "WakaGraphs"]

RUN dotnet restore "WakaGraphs/WakaGraphs.csproj" --runtime alpine-x64
COPY . .
RUN dotnet publish "WakaGraphs/WakaGraphs.csproj" -c Release --runtime alpine-x64 -o /app/publish --no-restore --self-contained true -p:PublishTrimmed=true -p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/app/WakaGraphs"]