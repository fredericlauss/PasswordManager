FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PasswordManager.Web/PasswordManager.Web/PasswordManager.Web.csproj", "PasswordManager.Web/"]
COPY ["PasswordManager.Web/PasswordManager.Web.Client/PasswordManager.Web.Client.csproj", "PasswordManager.Web.Client/"]
RUN dotnet restore "PasswordManager.Web/PasswordManager.Web.csproj"
COPY . .
WORKDIR "/src/PasswordManager.Web"
RUN dotnet build "PasswordManager.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PasswordManager.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PasswordManager.Web.dll"] 