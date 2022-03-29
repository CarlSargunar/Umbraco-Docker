# Umbraco and Docker

This repository is a guide for how to run Umbraco in Docker containers.

## Prerequisites to run this sample:

- Docker Desktop (on Windows or Mac)
    - WSL for windows (to run Linux containers)
- Visual Studio Code is helpful
    - There's also a plugin for VS Code Docker which is useful
- .NET 5 SDK. This will also work with 6

## Part 1 : Running a basic container in Docker

This part will cover

- Running a basic Umbraco 9 site
- Creating a docker container for the database sever.
- Converting the application to run in a docker container.

The copy for the Skrift Article to go along with the repository is in [Article_1.md](Article_1.md).

Once you have cloned the repository, follow the instructions in [Instructions_1.md](Instructions_1.md)

# References

## Docker-compose 

- https://docs.docker.com/compose/reference/

## Umbraco Docker    

- https://swimburger.net/blog/umbraco/how-to-run-umbraco-9-as-a-linux-docker-container
- https://codeshare.co.uk/blog/umbraco-9-useful-snippets/    

## Networking
    
- https://www.tutorialworks.com/container-networking/

## .NET
    
- https://github.com/dotnet/dotnet-docker/tree/main/samples/aspnetapp

## Database
    
- https://bigdata-etl.com/how-to-run-microsoft-sql-server-database-using-docker-and-docker-compose/
- https://www.abhith.net/blog/create-sql-server-database-from-a-script-in-docker-compose/

## Running on Linux:

- https://our.umbraco.com/forum/umbraco-9/107393-unable-to-deploy-v9-site-to-linux-web-app-on-azure

Line Endings:

- https://www.aleksandrhovhannisyan.com/blog/crlf-vs-lf-normalizing-line-endings-in-git/
