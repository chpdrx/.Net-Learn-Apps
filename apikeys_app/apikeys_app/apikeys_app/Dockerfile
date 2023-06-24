#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["apikeys_app.csproj", "apikeys_app.csproj"]
RUN dotnet restore "apikeys_app.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "apikeys_app.csproj" -c Release -o /app/build
ENTRYPOINT ["dotnet", "apikeys_app.dll"]

FROM build AS publish
RUN dotnet publish "apikeys_app.csproj" -c Release -o /app/publish
ENTRYPOINT ["dotnet", "apikeys_app.dll"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "apikeys_app.dll"]