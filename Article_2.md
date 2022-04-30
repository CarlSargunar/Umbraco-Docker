# Umbraco and Docker - Part Deux : The Difficult Second Album

Previously I covered the basic concepts of Docker. If you followed the examples through you would have created a database container, and a website container running Umbraco 9, and run them together in the same Docker network. I didn't cover networking, or another concept - Dockerfiles in a lot of detail as I wanted the first part to haev a low barrier to entry. 

In this second part we will cover these concepts in a bit more detail, and cover a couple of other concepts including : 

- Docker Volumes : A way to share data between containers, and to persist data between restarts
- Docker Networking : More details around how to connect containers to each other
- DockerFile : Defining the componentsand build steps required to build a container image
- Docker Compose : A tool to manage multiple containers in a single file

## Prereqisites

It's expected that if you have followed the first part of this tutorial, you have already installed Docker, and have created a database container, and a website container running Umbraco 9. This article will build on the code used from article 1, so if you haven't completed that first part, you should go back to [Article_1.](./Article_1.md)

## Umbraco Version

This article uses version 9 of Umbraco, which does not currently support SQLite, but this is a feature of Umbraco 10, which will be released during Codegarden 2022. I will update this github repo to use Umbraco 10 when it is released.


## It's all about storage

Containers have a problem - they can be sometimes ephemeral, and if they get deleted for any reason their file contents are gone. You don't want to think of them as analogue to a physical or a virtual server. If you were to delete a continer instance and recreate it, you  lose all data which has changed from the original continer image - it's not persisted, and that's by design. 

This may not be a problem when you are hosting a static website where all code and images are built into the image, but if you were hosting a database server in the container, that's less useful - any databases which were created after the container was created will be lost.

There are several options for managing storage in containers, and these are through Mounts, and there are 3 main types :

![Types of Mounts](/media/types-of-mounts.png)

- Volumes : These are stored in the host system filesystem managed by Docker, and typically these aren't available to non-docker processes. This is the recommended way to access storage outside the container instance
- Bind Mounts : These are basically like Volumes, but are not restricted - so they can be anywhere on the host system. This is useful if you want interaction between processes on the container as well as processes on the host system.
- tmpfs : These are stored in the Memory of the host system, and are never written to the filesystem. THey are by that nature extremely fast, and are useful for temporary storage, but should not be used for long-term storage.

I will focus on Volumes, as they are the most common option, and the recommended way to access storage outside the container instance, and used in the vast majority of cases.

## Docker Volumes

If you delete a container with a volume, any data which is stored in that volume will persist even when it is deleted, but any data stored in the container is lost. Additionally if multiple containers use the same volume, they will share that data. This has the added benefit of making it easier to share data between containers, and to persist data between restarts.

Volumes can be created in several ways, but for simplicity I will focus on 2 methods. 

- Using the docker run -v command
- Using docker compose files (see later in this article)

In the first part of this series we ran the umbraco website container using this command

    docker run --name umbdock -p 8000:80 -v media:/app/wwwroot/media -v logs:/app/umbraco/Logs -e ASPNETCORE_ENVIRONMENT='Staging' --network=umbNet -d umbdock

This created a volume for the umbraco logs diretory and the media folder - both would persist between restarts. 

# Networks

In the previous part I covered a little about docker networks, and I'll add a little to it in this part - but there's a lot more to networking and it's a huge topic, so I'm only going to cover enough to be dangerous 😊.

## Ports

Before diving into networks, we need to touch on ports - when a container is created, you define which ports internally are accessable outside the container. If you don't, the container will still run, but it won't be accessible from outside the container. In the CLI, ports are exposed using the -p flag, and are defined as host:container.

In our previous example we exposed port 8000 externally mapped to port 80 internally for the website containers, and port 1400 externally mapped internally to port 1433 for the database container.

    docker run --name umbdock -p 8000:80 -v media:/app/wwwroot/media -v logs:/app/umbraco/Logs -e ASPNETCORE_ENVIRONMENT='Staging' --network=umbNet -d umbdock

    docker run --name umbdata -p 1400:1433 --volume sqlserver:/var/opt/sqlserver -d umbdata

## Bridge Network 

The default network in docker is "bridge" networking, which is automatically assigned to containers unless specified. All containers in this network appear on the host IP address, but that also means that if you have multiple website containers, you can't use the same extenal port for all of them - you will need to map a different port per container. 

### Default Bridge Network

The standard bridge network also doesn't let the containers communicate with each other usnig DNS, only by their internal IP address, and that's not something that's available to you at design time, only run time. If you wanted to keep using the bridge network, you will need to query the contianer IP address at runtime and use that to address it. Every container can see every other container in this network, but that ususally isn't a problem. The IP address assigned will also change on all container restarts, so you can't rely on it being the same every time.

### User Defined Bridge Network

The main difference with this sort is that you can specify a name for this network, but also that containers can access each other using their container name. This is great when you are creating a connectionstring for a website to access a database for example - so even though the IP address isn't going to be static, it doesn't matter. You access the container you need by it's name. 

These are also called Custom bridge networks, and the other thing docker does is isolate all custom bridge networks from all others. Any container outside the network won't be able to see containers in the network, they will be isolated.

## Host Networks

Host networking is used when you have a container which basically needs to communicate as if it's running natively on the host computer - it will be able to access the hosts network. That's evident when you create the container - you don't need to spricfy any port mappings. Ports on the container are natively mapped to the host computer network

There are other networking types available, but those are less common, and are more advanced, so I'm not going to cover them here - if you want to find out more there are reference links in the footer. 

## DockerFile and Docker Compose

Now that I have covered networking, and storage the last part, and possibly the most important covers Dockerfile and Docker Compose. The Dockerfile is a file which is used to build a container image, and the Docker Compose file is a file which is used to manage multiple containers in a single file. Thus you define each container in your application with it's on Dockerfile, and you define the application as a whole with 


### Dockerfile

In the previous article we created 2 docker containers, each with their own Dockerfiles. 


### Docker Compose



# References

- Docker Volumes :  https://docs.docker.com/storage/volumes/
- Docker Network : https://docs.docker.com/network/
    - Bridge Network : https://docs.docker.com/network/bridge/
    - Host Network : https://docs.docker.com/network/host/
- Dockerfile : https://docs.docker.com/engine/reference/builder/
- Docker Compose : https://docs.docker.com/compose/

## Further Reading

- Docker Swarm vs Kubernetes : https://circleci.com/blog/docker-swarm-vs-kubernetes/



# [WIP] Docker Compose

Docker Compose lets you define multiple containers as part of an application - copy the docker-compose.yml file from the Files folder to the root.

To start the sample run the folling command. 

    docker compose up -d

## To Clean up your images and re-build changes

Run the following. 

**NOTE - This will remove all unused images and volumes. If you have images from other applications you want to keep, don't run this.**

    docker compose down
    docker compose rm -f
    docker image prune -a -f 
    docker volume prune -f 