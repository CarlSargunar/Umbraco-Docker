# Notes

Docker Sample for Sql Server + Umbraco on Docker. Prerequisites to run this sample:

- Docker Desktop (on Windows or Mac)
    - WSL for windows (to run Linux containers)
- Visual Studio Code is helpful

To start the sample run the folling command. 

    docker compose up -d

You will then be able to browse the site by opening the following URL

    http://localhost:5080/

*Note : This sample does not set up the HTTPS certificate. That's intentional, becuase it's much harder, and not necessary for this proof of concept.*

To log into the Umbraco Back office, you will need to use the following URL and  credentials:

    Username: admin@admin.com
    Password: Pa55word!!

    http://localhost:5080/umbraco


# TODO Still

- Make the Dockerfile for the Weather API simpler - the current default one is hard to read
- Make the URL of the API call a configurable parameter : in homecontroller.cs

## To Clean up your images and re-build changes

Run the following. 

**NOTE - This will remove all unused images and volumes. If you have images from other applications you want to keep, don't run this.**

    docker compose down
    docker compose rm -f
    docker image prune -f 
    docker volume prune -f 


## Setup Process

If you want to re-create this from scratch, you can do the following. You'll need to add the customised controllers and projects etc to call the API, but the following instructions were used to create the project and references

### Ensure we have the latest Umbraco templates
    dotnet new -i Umbraco.Templates

### Create solution/project
    dotnet new globaljson --sdk-version 5.0.404
    dotnet new sln --name UmbDock

### Add the Umbraco Project

    dotnet new umbraco -n UmbDock --friendly-name "Admin User" --email "admin@admin.com" --password "Pa55word!!" --connection-string "Server=db-container;Database=umbraco;User Id=sa;Password=SQL_password123;"
    dotnet sln add UmbDock
    dotnet add UmbDock package Portfolio
    dotnet add package Newtonsoft.Json

### Modify csProj

Edit the csproj file to change following element:

    <!-- Force windows to use ICU. Otherwise Windows 10 2019H1+ will do it, but older windows 10 and most if not all winodws servers will run NLS -->
    <ItemGroup Condition="'$(OS)' == 'Windows_NT'">
        <PackageReference Include="Microsoft.ICU.ICU4C.Runtime" Version="68.2.0.9" />
        <RuntimeHostConfigurationOption Include="System.Globalization.AppLocalIcu" Value="68.2" />
    </ItemGroup>

Without this step, the project won't compile on Linux, but will compile in windows.


### Also Create the Email Api

    dotnet new webapi -o WeatherApi  
    dotnet sln add WeatherApi

# Notes

## Docker for Database

To run a database in a standalone container, you can use the following command:

    docker run -d --name sql_server -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=SQL_password123' -v mssqlsystem:/var/opt/mssql -v mssqluser:/var/opt/sqlserver -p 1433:1433 mcr.microsoft.com/mssql/server:2017-latest


To run the website in isolation, from the root of the project you can run:

    dotnet run --project UmbDock

# Reference


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