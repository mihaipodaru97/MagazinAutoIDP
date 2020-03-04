#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MagazinAuto/MagazinAuto.csproj", "MagazinAuto/"]
RUN dotnet restore "MagazinAuto/MagazinAuto.csproj"
COPY . .
WORKDIR "/src/MagazinAuto"
RUN dotnet build "MagazinAuto.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MagazinAuto.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MagazinAuto.dll"]