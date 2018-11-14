FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Convenio.API/Convenio.API.csproj Convenio.API/
RUN dotnet restore Convenio.API/Convenio.API.csproj
COPY . .
WORKDIR /src/Convenio.API
RUN dotnet build Convenio.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Convenio.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Convenio.API.dll"]
