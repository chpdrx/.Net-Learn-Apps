#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["resource_app.csproj", "resource_app.csproj"]
RUN dotnet restore "resource_app.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "resource_app.csproj" -c Release -o /app/build
ENTRYPOINT ["dotnet", "resource_app.dll"]

FROM build AS publish
RUN dotnet publish "resource_app.csproj" -c Release -o /app/publish
ENTRYPOINT ["dotnet", "resource_app.dll"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "resource_app.dll"]