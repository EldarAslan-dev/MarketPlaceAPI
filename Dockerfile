FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY MarketPlace.Api/MarketPlace.Api.csproj MarketPlace.Api/
RUN dotnet restore MarketPlace.Api/MarketPlace.Api.csproj
COPY MarketPlace.Api/ MarketPlace.Api/
RUN dotnet publish MarketPlace.Api/MarketPlace.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MarketPlace.Api.dll"]
