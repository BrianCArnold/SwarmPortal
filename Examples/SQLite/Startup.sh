#!/bin/bash
if (docker node ls 2&>1 > /dev/null) then 
    echo "You're in swarm mode."
else 
    echo "Not in docker swarm mode, or a worker node. Try 'docker swarm init'?"
    return;
fi

if (docker network inspect webhosts 2&>1 > /dev/null) then
    echo "Webhosts network found. (Better be overlay driver)"
else
    echo "Webhosts network not found. Try 'docker network create --driver overlay webhosts'."
    return;
fi

echo "Making sure necessary directories exist..."
sudo mkdir -p /var/opt/keycloak/db
sudo mkdir -p /var/opt/swarmportal/persist
sudo mkdir -p /var/opt/swarmportal/db
sudo mkdir -p /var/opt/traefik

echo "Copying over configuration files..."
sudo cp var/opt/swarmportal/persist/settings.json /var/opt/swarmportal/persist/settings.json
sudo cp var/opt/traefik/traefik.yml /var/opt/traefik/traefik.yml

echo "Setting permissions..."
sudo chown 1000:0 -R /var/opt/keycloak/db
sudo chown 1000:0 -R /var/opt/swarmportal/persist
sudo chown 10001:0 -R /var/opt/swarmportal/db
sudo chown 1000:0 -R /var/opt/traefik

echo "All done, I created a directory for SqlServer as well, if you need it."

echo "Starting up the stack..."
docker stack deploy -c docker-compose.yml traefik

echo "Done. Go to http://keys.swarmportal.com to log in as admin/admin, and follow the remaining instructions to create a client."