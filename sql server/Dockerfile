# make user root when starting container -u 0:0 or user : root in docker-compose

FROM mcr.microsoft.com/mssql/server:2022-latest as base 

USER root
ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=your_Strong_Password1
ENV MSSQL_PID=Express

EXPOSE 14444:1433

WORKDIR /var/opt/mssql/data
COPY [".","."] 

RUN chmod +x /var/opt/mssql/data/entrypoint.sh
RUN chmod +x /var/opt/mssql/data/configure-db.sh

ENTRYPOINT ["./entrypoint.sh"]


