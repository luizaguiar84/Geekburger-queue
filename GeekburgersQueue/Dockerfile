﻿FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GeekburgersQueue/GeekburgersQueue.csproj", "GeekburgersQueue/"]
RUN dotnet restore "GeekburgersQueue/GeekburgersQueue.csproj"
COPY . .
WORKDIR "/src/GeekburgersQueue"
RUN dotnet build "GeekburgersQueue.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GeekburgersQueue.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GeekburgersQueue.dll"]
