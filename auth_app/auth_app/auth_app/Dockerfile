#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["auth_app.csproj", "auth_app.csproj"]
RUN dotnet restore auth_app.csproj
COPY . .
WORKDIR "/src"
RUN dotnet build auth_app.csproj -c Release -o /app/build
ENTRYPOINT ["dotnet", "auth_app.dll"]

FROM build AS publish
RUN dotnet publish auth_app.csproj -c Release -o /app/publish
ENTRYPOINT ["dotnet", "auth_app.dll"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "auth_app.dll"]