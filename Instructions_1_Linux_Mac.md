# Instructions for Docker in Umbraco Part on Linux and Mac

These instructions are specific to Linux and Mac, which needs to be done in a slightly different order, since you can't create the Umbraco site without having the database in place, since LocalDB does not work outsider Windows.

## 1 - Setup the Database Container

Follow these instructions to setup the database container.

- Copy the entire UmbData folder from Files/UmbData into the Root.
- Delete the following files
    - setup.sql
    - startup.sh

### Amend the Development Connectionstring

Update the developement string to use the new Database Container. Replace the relevant line in the file with the following. Note : The port is using 1400 as a non-standard port in case there is already SQL Server running on the local machine. 

    "umbracoDbDSN": "Server=localhost,1400;Database=UmbracoDb;User Id=sa;Password=SQL_password123;"

### Build the database image. 

Create a docker image for the database with the tag umbdata. Note : tags need to be lower case.

    docker build --tag=umbdata .\UmbData

Run the database container with the ame umbdata. We're using a non-standard port in case you have a local SQL server.

    docker run --name umbdata -p 1400:1433 --volume sqlserver:/var/opt/sqlserver -d umbdata

## 2 - Creating a new Umbraco Website 

These are the instructions for Windows. If you are on a mac or linux machine you will need to follow the intructions in [Instructions_1_linux_mac.md](Instructions_1_linux_mac.md).

### Ensure we have the latest Umbraco templates

    dotnet new -i Umbraco.Templates

### Set the SDK Version being used and Create solution/project

    dotnet new globaljson --sdk-version 5.0.404
    dotnet new sln --name UmbDock

### Start a new Umbraco website project

    dotnet new Umbraco -n UmbDock --friendly-name "Admin User" --email "admin@admin.com" --password "1234567890" --connection-string "Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Umbraco.mdf;Integrated Security=True"

Instructions for Linux or Mac:

    dotnet new Umbraco -n UmbDock

### Add the project to the solution, and install a starter kit

    dotnet sln add UmbDock
    dotnet add UmbDock package Clean

### Modify csProj

Edit the csproj file to change following element:

    <!-- Force windows to use ICU. Otherwise Windows 10 2019H1+ will do it, but older windows 10 and most if not all winodws servers will run NLS -->
    <ItemGroup Condition="'$(OS)' == 'Windows_NT'">
        <PackageReference Include="Microsoft.ICU.ICU4C.Runtime" Version="68.2.0.9" />
        <RuntimeHostConfigurationOption Include="System.Globalization.AppLocalIcu" Value="68.2" />
    </ItemGroup>

Without this step, the project won't compile on Linux, but will compile in windows - This is needed even if you are using windows, since eventually the application will run in a docker container on Linux.

### Now Run the site

This will also create the database as a LocalDB

    dotnet run --project UmbDock

In the output you willl see which port the site is running on. You should be able to browse to that site on any browser. You need to complete this step so that the databases are created.



### Test the site still works

At this point you can run the local site again, but it will talk to the Database container rather than the local DB

    dotnet run --project UmbDock

As before, the command will display which port should be used to browse for the site.

## 3 - Convert the website into a container

### Create the Dockerfile

Copy the Docker file by copying it from /Files/Umbdata/UmbDock/Dockerfile to UmbDock/Dockerfile.

### Network

We need to define a custom bridge network to run this application under

    docker network create -d bridge umbNet

We will then connect our existing container to that network

    docker network connect umbNet umbdata

Create a staging config file on the website called appsettings.Staging.json by copying the existing appsettings.Development.json file. 

Amend the connectionstring with the following. You will need the non-standard port setting 

    "umbracoDbDSN": "Server=umbdata;Database=UmbracoDb;User Id=sa;Password=SQL_password123;"

### Create the website container image

We've got our site, now we need to build an image which can be used to host the application. From the UmbDock folder run the following

    docker build --tag=umbdock .\UmbDock

To run a single local instance

    docker run --name umbdock00 -p 8000:80 -v media:/app/wwwroot/media -v logs:/app/umbraco/Logs -e ASPNETCORE_ENVIRONMENT='Staging' --network=umbNet -d umbdock



## References


Slides : [https://docs.google.com/presentation/d/1RhfTbOapkVlpEyAjJs80bQEh4oOKxCMD7Y8lyB3sL7E/](https://docs.google.com/presentation/d/1RhfTbOapkVlpEyAjJs80bQEh4oOKxCMD7Y8lyB3sL7E/)    

