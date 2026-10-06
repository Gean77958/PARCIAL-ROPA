FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

# Program.cs ya lee la variable PORT que asigna Render, por eso aquí no se
# fija ASPNETCORE_URLS (sería redundante). EXPOSE solo documenta el puerto.
EXPOSE 10000

ENTRYPOINT ["dotnet", "TiendaApi.dll"]
