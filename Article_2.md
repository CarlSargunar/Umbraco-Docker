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