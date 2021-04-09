FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.12 AS publish
WORKDIR /src
COPY ["WakaGraphs/WakaGraphs.csproj", "WakaGraphs/"]
COPY ["WakaGraphs.Templates/WakaGraphs.Templates.csproj", "WakaGraphs.Templates/"] 

COPY . .
RUN dotnet publish "WakaGraphs/WakaGraphs.csproj" -c Release --runtime alpine-x64 -o /app/publish --self-contained true -p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine3.12 AS final
RUN apk add --no-cache git

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/app/WakaGraphs"]