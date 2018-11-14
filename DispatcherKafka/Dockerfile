FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY DispatcherKafka/DispatcherKafka.csproj DispatcherKafka/
RUN dotnet restore DispatcherKafka/DispatcherKafka.csproj
COPY . .
WORKDIR /src/DispatcherKafka
RUN dotnet build DispatcherKafka.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish DispatcherKafka.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DispatcherKafka.dll"]
