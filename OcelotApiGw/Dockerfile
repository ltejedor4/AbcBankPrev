FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY OcelotApiGw/OcelotApiGw.csproj OcelotApiGw/
RUN dotnet restore OcelotApiGw/OcelotApiGw.csproj
COPY . .
WORKDIR /src/OcelotApiGw
RUN dotnet build OcelotApiGw.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish OcelotApiGw.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OcelotApiGw.dll"]
