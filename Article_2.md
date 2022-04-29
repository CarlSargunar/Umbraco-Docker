# Umbraco and docker - Part Deux : The difficuly second album

In the last part, I covered the basic concepts of Docker. If you followed the examples through you would have created a database container, and a website container running Umbraco 9, and run them together in the same Docker network. I didn't cover networking, or another concept - Dockerfiles in a lot of detail as I wanted the first part to haev a low barrier to entry. 

In this second part we will cover these concepts in a bit more detail, and cover a couple of other concepts including : 

- Docker Volumes : A way to share data between containers, and to persist data between restarts
- Docker Networking : More details around how to connect containers to each other
- Docker Compose : A tool to manage multiple containers in a single file

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

Volumes have several advantages over bind mounts:

- Volumes are easier to back up or migrate than bind mounts.
- You can manage volumes using Docker CLI commands or the Docker API.
- Volumes work on both Linux and Windows containers.
- Volumes can be more safely shared among multiple containers.
- New volumes can have their content pre-populated by a container.

Volumes can be created in several ways, but for simplicity I will focus on 2 methods. 

- Using the docker run -v command
- Using docker compose files (see later in this article)

In the first part of this series we ran the umbraco website container using this command

    docker run --name umbdock -p 8000:80 -v media:/app/wwwroot/media -v logs:/app/umbraco/Logs -e ASPNETCORE_ENVIRONMENT='Staging' --network=umbNet -d umbdock

This created a volume for the umbraco logs diretory and the media folder - both would persist between restarts. 



# References

- Docker Volumes :  https://docs.docker.com/storage/volumes/




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