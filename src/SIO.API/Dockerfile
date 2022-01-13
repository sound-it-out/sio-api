#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src", "src"]
RUN dotnet restore "src/SIO.Api/SIO.Api.csproj"
COPY . .
WORKDIR "/src/src/SIO.Api"
RUN dotnet build "SIO.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SIO.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SIO.Api.dll"]