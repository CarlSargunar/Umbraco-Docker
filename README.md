# Notes

Docker Sample for Sql Server + Umbraco on Docker. Prerequisites to run this sample:

- Docker Desktop (on Windows or Mac)
    - WSL for windows (to run Linux containers)
- Visual Studio Code is helpful
    - There's also a plugin for VS Code Docker which is useful


Slides : [https://docs.google.com/presentation/d/1RhfTbOapkVlpEyAjJs80bQEh4oOKxCMD7Y8lyB3sL7E/](https://docs.google.com/presentation/d/1RhfTbOapkVlpEyAjJs80bQEh4oOKxCMD7Y8lyB3sL7E/)

## Setup Process

If you want to re-create this from scratch, you can do the following. You'll need to add the customised controllers and projects etc to call the API, but the following instructions were used to create the project and references

### Ensure we have the latest Umbraco templates
    dotnet new -i Umbraco.Templates

### Create solution/project

    dotnet new globaljson --sdk-version 5.0.404
    dotnet new sln --name UmbDock

### Add the Umbraco Project

    dotnet new Umbraco -n UmbDock --friendly-name "Admin User" --email "admin@admin.com" --password "1234567890" --connection-string "Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Umbraco.mdf;Integrated Security=True"

if you are on Linux, you will need acess to a SQL server instance - either standalone or running in Docker. In that case replace the Connectionstring with one to a valid database

### Add the project to the solution, and install a starter kit

    dotnet sln add UmbDock
    dotnet add UmbDock package Clean

# Now Run the site

This will also create the database as a LocalDB

    dotnet run --project UmbDock

# Turning the site into a Docker App

## Modify csProj

Edit the csproj file to change following element:

    <!-- Force windows to use ICU. Otherwise Windows 10 2019H1+ will do it, but older windows 10 and most if not all winodws servers will run NLS -->
    <ItemGroup Condition="'$(OS)' == 'Windows_NT'">
        <PackageReference Include="Microsoft.ICU.ICU4C.Runtime" Version="68.2.0.9" />
        <RuntimeHostConfigurationOption Include="System.Globalization.AppLocalIcu" Value="68.2" />
    </ItemGroup>

Without this step, the project won't compile on Linux, but will compile in windows.

# Setup the Database Container

Copy the UmbData folder from Files into the Root. This has the data container set-up in it

The actual database we created in a local DB file for now  - so let's MOVE the MDF files into a Docker database container and run them from in there.

These files are Umbraco.mdf and Umbraco_log.ldf in the folder UmbDock\umbraco\Data

Don't forget to amend the Dev connectionstring in appsettings.Development.json

    "umbracoDbDSN": "Server=localhost,1400;Database=UmbracoDb;User Id=sa;Password=SQL_password123;"


## Database

Build the image. Tags need to be lower case

    docker build --tag=umbdata .\UmbData

Run the database container. We're using a non-standard port in case you have a local SQL server.

    docker run --name umbdata -p 1400:1433 --volume sqlserver:/var/opt/sqlserver -d umbdata

At this point you can run the local site again, but it will talk to the Database container rather than the local DB

    dotnet run --project UmbDock

# Application Container

We would like to run this website in a container too - let's test that container.

First create the Docker file by copying it from /Files/Umbdata/UmbDock/Dockerfile to the UmbDock folder

## Network

We need to define a custom bridge network to run this application under

    docker network create -d bridge umbNet

We will then connect our existing container to that network

    docker network connect umbNet umbdata

Create a staging config file called appsettings.Staging.json and put this into a Staging Connectionstring with a transform. You will need the non-standard port setting 

    "umbracoDbDSN": "Server=umbdata;Database=UmbracoDb;User Id=sa;Password=SQL_password123;"

We've got our site, now we need to build an image which can be used to host the application. From the UmbDock folder run the following

    docker build --tag=umbdock .\UmbDock

To run a single local instance

    docker run --name umbdock00 -p 8000:80 -v media:/app/wwwroot/media -v logs:/app/umbraco/Logs -e ASPNETCORE_ENVIRONMENT='Staging' --network=umbNet -d umbdock


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
