FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 4444

#mount /etc instead of /run/systemd/resolve on container
RUN apt-get update -y && \
    apt-get upgrade -y && \
    apt-get install -y git pkg-config libcap-dev libsystemd-dev && \
    apt-get install -y build-essential

RUN apt-get install -y sudo && \
    apt-get install -y pip && \
    pip install pipreqs && \
    mkdir ~/hitsunittest && \
    git clone https://github.com/yahya-abdul-majeed/hits.unittest.git ~/hitsunittest

RUN mkdir -p /home/yahya/isolate && \
   cd /home/yahya/isolate && \
   git clone -b v1.10.1 https://github.com/ioi/isolate.git . && \
   make install
   

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./crow.csproj","crow/"] 
RUN dotnet restore "crow/crow.csproj"
COPY [".","crow/"]
WORKDIR /src/crow
RUN dotnet build "crow.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./crow.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
COPY --from=publish /app/publish .
CMD ["dotnet","crow.dll","--urls","http://+:4444"]
