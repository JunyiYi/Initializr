FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
COPY ["Company.WebApplication1.csproj", "."]
RUN dotnet restore "Company.WebApplication1.csproj"
COPY . .
RUN dotnet build "Company.WebApplication1.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Company.WebApplication1.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Company.WebApplication1.dll"]
