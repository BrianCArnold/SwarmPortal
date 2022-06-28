These instructions are for a Docker Swarm environment.

0. Make sure your Docker Swarm environment is active
    - docker node ls
      - This should show you at least one node. If it says it's not part of a swarm, initialize the swarm
    - docker swarm init
      - Now create a webhosts network for all applications to communicate on:
    - docker network create -d overlay webhosts
      - This is necessary for traefik to communicate with other containers from other nodes and stacks.

1. Create directories, set permissions, and copy traefik.yml and settings.json
    - sudo mkdir -p /var/opt/keycloak/db
    - sudo mkdir -p /var/opt/swarmportal/persist
    - sudo mkdir -p /var/opt/swarmportal/db
    - sudo mkdir -p /var/opt/traefik
    - sudo cp var/opt/swarmportal/persist/settings.json /var/opt/swarmportal/persist/settings.json
    - sudo cp var/opt/traefik/traefik.yml /var/opt/traefik/traefik.yml
    - sudo chown 1000:0 -R /var/opt/keycloak/db
    - sudo chown 1000:0 -R /var/opt/swarmportal/persist
    - sudo chown 10001:0 -R /var/opt/swarmportal/db
    - sudo chown 1000:0 -R /var/opt/traefik
2. Start Stack
    - docker stack deploy -c docker-compose.yml traefik
3. Create Client on http://keys.swarmportal.com
    - Click Administration Console
    - Click on "Clients" on left side
    - Click "Create"
        - Client ID: SwarmPortal
        - Client Protocol: openid-connect
        - Root URL: <blank>
4. After the client is created:
    - Set Login Theme (optional)
    - Enable Implicit Flow
    - Set valid Redirect URIs (for simplicity, use "\*" as an example, but this is **__*insecure*__**.)
    - Set Web Origins (http://home.swarmportal.com)
    - Expand "Authentication Flow Overrides
        - Browser Flow: "browser"
        - Direct Grant Flow: "direct grant"
5. Create Roles
    - Add "Infrastructure" and "Status" to the Roles tab for Client.
6. Map Client Roles
    - Under Mappers tab for Client
        - Create
            - Name: "Client Roles" (any)
            - Mapper Type: "User Client Role"
            - Client ID: "SwarmPortal"
            - Client Role Prefix: <blank>
            - Token Claim Name: "roles"
            - Claim JSON Type: "string"
            - Add to ID Token: On
            - Add to Access Token: On
            - Add to userinfo: On

7. Add test user with only "Status" Role
    - Manage Users (Realm Level, on the left, under Manage, Click "Users")
    - Add User
        - Username: "status"
    - Set password in Credentials Tab for newly created user
        - Password: "st4tus"
        - Password Confirmation: "st4tus"
    - Add Client roles in Roles tab
        - Select "SwarmPortal" in "Client Roles" Drop Down
        - Add "Status" roll.
        
8. Add test user with only "Infrastructure" Role
    - Manage Users (Realm Level, on the left, under Manage, Click "Users")
    - Add User 
        - Username: "infrastructure"
    - Set password in Credentials Tab for newly created user
        - Password: "1nfr4structur3"
        - Password Confirmation: "1nfr4structur3"
    - Add Client roles in Roles tab
        - Select "SwarmPortal" in "Client Roles" Drop Down
        - Add "Infrastructure" roll.
        
9. Add test user with "Status" and "Infrastructure" Roles
    - Manage Users (Realm Level, on the left, under Manage, Click "Users")
    - Add User
        - Username: "admin"
    - Set password in Credentials Tab for newly created user
        - Password: "4dm1n"
        - Password Confirmation: "4dm1n"
    - Add Client roles in Roles tab
        - Select "SwarmPortal" in "Client Roles" Drop Down
        - Add "Infrastructure" roll.
        - Add "Status" roll.
        
10. Restart Stack
    - docker stack rm traefik
    - docker stack deploy -c docker-compose.yml traefik
