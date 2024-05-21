#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 3333 

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./lynx.csproj", "lynx/"]
RUN dotnet restore "lynx/lynx.csproj"
COPY . ./lynx
WORKDIR "/src/lynx"
RUN dotnet build "lynx.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lynx.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "lynx.dll","--urls","http://+:3333"]