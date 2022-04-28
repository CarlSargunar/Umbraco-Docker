# Umbraco and docker - Part Deux : The difficuly second album

In the last part, I covered the basic concepts of Docker. If you followed the examples through you would have created a database container, and a website container running Umbraco 9, and run them together in the same Docker network. I didn't cover networking, or another concept - Dockerfiles in a lot of detail as I wanted the first part to haev a low barrier to entry. 

In this second part we will cover these concepts in a bit more detail, and cover a couple of other concepts including : 

- Docker Compose : A tool to manage multiple containers in a single file
- Docker Volumes : A way to share data between containers, and to persist data between restarts
- Docker Networking : More details around how to connect containers to each other

## It's all about storage

Containers have a problem - they work best when they're not seen as long-running, permanemt processes. You don't want to think of them as analogue to a physical or a virtual server. If you were to delete a continer instance and recreate it, you would lose any data which had changed from the original continer image - it's not persisted, and that's by design. 

This may not be a problem when you are hosting a static website where all code and images are built into the image, but if you were hosting a database server in the container, that's less useful - any databases which were created after the container was created will be lost.

There are several options for managing storage in containers, and these are through Mounts, and there are 3 main types :

![Types of Mounts](/media/types-of-mounts.png)

- Volumes : These are stored in the host system filesystem managed by Docker, and typically these aren't available to non-docker processes. This is the recommended way to access storage outside the container instance
- Bind Mounts : These are basically like Volumes, but are not restricted - so they can be anywhere on the host system. This is useful if you want interaction between processes on the container as well as processes on the host system.
- tmpfs : These are stored in the Memory of the host system, and are never written to the filesystem. THey are by that nature extremely fast, and are useful for temporary storage, but should not be used for long-term storage.

I will focus on Volumes, as they are the most common option, and the recommended way to access storage outside the container instance.

## Creating Volumes

That's where Volumes come in - a docker volume lets you specity a path on the host machine, and then mount that path into the container. This means that when you delete the container, the data is not lost as long as the volume is not deleted. Additionally, if you use the same volume with more than one container, they will share the same data. This has the added benefit of making it easier to share data between containers, and to persist data between restarts.



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