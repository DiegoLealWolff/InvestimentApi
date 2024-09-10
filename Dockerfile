FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
EXPOSE 443
WORKDIR /app

COPY ["InvestimentApi.csproj", "."]
RUN dotnet restore "./InvestimentApi.csproj"

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "InvestimentApi.dll"]