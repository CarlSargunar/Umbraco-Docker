FROM mcr.microsoft.com/mssql/server:2017-latest

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=SQL_password123
ENV MSSQL_PID=Express

COPY . /

ENTRYPOINT [ "/bin/bash", "startup.sh" ]
CMD [ "/opt/mssql/bin/sqlservr" ]