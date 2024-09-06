#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["InvestimentApi/InvestimentApi.csproj", "InvestimentApi/"]
RUN dotnet restore "InvestimentApi/InvestimentApi.csproj"
COPY . .
WORKDIR "/src/InvestimentApi"
RUN dotnet build "InvestimentApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InvestimentApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InvestimentApi.dll"]