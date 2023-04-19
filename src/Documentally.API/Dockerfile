#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Documentally.API/Documentally.API.csproj", "Documentally.API/"]
RUN dotnet restore "Documentally.API/Documentally.API.csproj"
COPY . .
WORKDIR "/src/Documentally.API"
RUN dotnet build "Documentally.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Documentally.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Documentally.API.dll"]