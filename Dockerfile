FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/Playtestr.Api/Playtestr.Api.csproj src/Playtestr.Api/
RUN dotnet restore src/Playtestr.Api/Playtestr.Api.csproj
COPY src/ src/
RUN dotnet publish src/Playtestr.Api -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Playtestr.Api.dll"]