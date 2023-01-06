FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY src/MissBot.Domain/MissBot.Domain.csproj ./src/MissBot.Domain/
COPY src/MissBot.Application/MissBot.Application.csproj ./src/MissBot.Application/
COPY src/MissBot.Infrastructure/MissBot.Infrastructure.csproj ./src/MissBot.Infrastructure/
COPY src/MissBot.Api/MissBot.Api.csproj ./src/MissBot.Api/
RUN dotnet restore ./src/MissBot.Api/MissBot.Api.csproj

COPY . ./
RUN dotnet publish ./src/MissBot.Api -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
EXPOSE 5432
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "MissBot.Api.dll"]
