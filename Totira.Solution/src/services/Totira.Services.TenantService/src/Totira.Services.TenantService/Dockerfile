#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["services/Totira.Services.TenantService/src/Totira.Services.TenantService/Totira.Services.TenantService.csproj", "services/Totira.Services.TenantService/src/Totira.Services.TenantService/"]
RUN dotnet restore "services/Totira.Services.TenantService/src/Totira.Services.TenantService/Totira.Services.TenantService.csproj"
COPY . .
WORKDIR "/src/services/Totira.Services.TenantService/src/Totira.Services.TenantService"
RUN dotnet build "Totira.Services.TenantService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Totira.Services.TenantService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Totira.Services.TenantService.dll"]