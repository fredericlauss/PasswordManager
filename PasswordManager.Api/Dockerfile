FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PasswordManager.Api/PasswordManager.Api.csproj", "PasswordManager.Api/"]
COPY ["PasswordManager.Core/PasswordManager.Core.csproj", "PasswordManager.Core/"]
RUN dotnet restore "PasswordManager.Api/PasswordManager.Api.csproj"
COPY . .
WORKDIR "/src/PasswordManager.Api"
RUN dotnet build "PasswordManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PasswordManager.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PasswordManager.Api.dll"] 