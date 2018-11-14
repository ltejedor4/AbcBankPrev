FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Pago.API/Pago.API.csproj Pago.API/
RUN dotnet restore Pago.API/Pago.API.csproj
COPY . .
WORKDIR /src/Pago.API
RUN dotnet build Pago.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Pago.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pago.API.dll"]
