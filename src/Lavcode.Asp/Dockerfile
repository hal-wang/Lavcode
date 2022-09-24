#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Lavcode.Asp/Lavcode.Asp.csproj", "Lavcode.Asp/"]
RUN dotnet restore "Lavcode.Asp/Lavcode.Asp.csproj"
COPY . .
WORKDIR "/src/Lavcode.Asp"
RUN dotnet build "Lavcode.Asp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lavcode.Asp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lavcode.Asp.dll"]