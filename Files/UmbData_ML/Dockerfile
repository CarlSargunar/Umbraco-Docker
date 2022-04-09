FROM mcr.microsoft.com/mssql/server:2019-GDR1-ubuntu-16.04

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=SQL_password123
ENV MSSQL_PID=Express

USER root

ENV MSSQL_BACKUP_DIR="/var/opt/sqlserver"
ENV MSSQL_DATA_DIR="/var/opt/sqlserver"
ENV MSSQL_LOG_DIR="/var/opt/sqlserver"

COPY setup.sql /
COPY startup.sh /

# Warning : This will delete any existing data in each run of the container
ENTRYPOINT [ "/bin/bash", "startup.sh" ]
CMD [ "/opt/mssql/bin/sqlservr" ]