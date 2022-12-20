#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Feed.Runner/Feed.Runner.csproj", "Feed.Runner/"]
COPY ["Feed.Runner.Lib/Feed.Runner.Lib.csproj", "Feed.Runner.Lib/"]
COPY ["Twitter.Api.Data/Twitter.Api.Data.csproj", "Twitter.Api.Data/"]
COPY ["Twitter.Api.Client/Twitter.Api.Client.csproj", "Twitter.Api.Client/"]

RUN dotnet restore "Feed.Runner/Feed.Runner.csproj"
COPY . .
WORKDIR "/src/Feed.Runner"
RUN dotnet build "Feed.Runner.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Feed.Runner.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Feed.Runner.dll"]