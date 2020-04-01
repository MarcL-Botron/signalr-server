FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Botron.Notification.Api/Botron.Notification.Api.csproj", "Botron.Notification.Api/"]
RUN dotnet restore "Botron.Notification.Api/Botron.Notification.Api.csproj"
COPY . .
WORKDIR "/src/Botron.Notification.Api"
RUN dotnet build "Botron.Notification.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Botron.Notification.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Botron.Notification.Api.dll"]