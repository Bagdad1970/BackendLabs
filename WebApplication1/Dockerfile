FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Common/Common.csproj Common/
COPY Messages/Messages.csproj Messages/
COPY Migrations/Migrations.csproj Migrations/
COPY Models/Models.csproj Models/
COPY WebApplication1/WebApplication1.csproj WebApplication1/

RUN dotnet restore "WebApplication1/WebApplication1.csproj"

COPY Common/ Common/
COPY Messages/ Messages/
COPY Migrations/ Migrations/
COPY Models/ Models/
COPY WebApplication1/ WebApplication1/

WORKDIR /src/WebApplication1
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApplication1.dll"]