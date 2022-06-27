These instructions are for a Docker Swarm environment.

0. Create directories, set permissions, and copy traefik.yml and settings.json
    - sudo mkdir -p /var/opt/keycloak/db
    - sudo mkdir -p /var/opt/swarmportal/persist
    - sudo mkdir -p /var/opt/traefik
    - sudo cp var/opt/swarmportal/persist/settings.json /var/opt/swarmportal/persist/settings.json
    - sudo cp var/opt/traefik/traefik.yml /var/opt/traefik/traefik.yml
    - sudo chown 1000:0 -R /var/opt/keycloak/db
    - sudo chown 1000:0 -R /var/opt/swarmportal/persist
    - sudo chown 1000:0 -R /var/opt/traefik
1. Start Stack
    - docker stack deploy -c docker-compose.yml traefik
2. Create Client
    - Client ID: SwarmPortal
    - Client Protocol: openid-connect
    - Root URL: <blank>
3. After the client is created:
    - Set Login Theme (optional)
    - Enable Implicit FlowSwarmPortal
    - Set valid Redirect URIs (for simplicity, use "*" as an example)
    - Set Web Origins (http://home.swarmportal.com)
    - Expand "Authentication Flow Overrides
        - Browser Flow: "browser"
        - Direct Grant Flow: "direct grant"
4. Create Roles
    - Add "Infrastructure" and "Status" to the Roles tab for Client.
5. Map Client Roles
    - Under Mappers tab for Client
        - Create
            - Name: "Client Roles" (any)
            - Mapper Type: "User Client Role"
            - Client ID: "SwarmPortal"
            - Client Role Prefix: <blank>
            - Token Claim Name: "roles"
            - Claim JSON Type: "string"
            - Add to ID Token: true
            - Add to Access Token: true
            - Add to userinfo: true
6. Add a Status only user
    - Manage Users (Realm)
        - Add User (Enter your preferred info)
        - Set password in Credentials
        - Add Client roles in Roles tab
            - Select "SwarmPortal" in Client Drop Down
            - Add Status roll.
        
7. Add a Infrastructure only user
    - Manage Users (Realm)
        - Add User (Enter your preferred info)
        - Set password in Credentials
        - Add Client roles in Roles tab
            - Select "SwarmPortal" in Client Drop Down
            - Add Infrastructure roll.
        
8. Add a Infrastructure and Status user
    - Manage Users (Realm)
        - Add User (Enter your preferred info)
        - Set password in Credentials
        - Add Client roles in Roles tab
            - Select "SwarmPortal" in Client Drop Down
            - Add Infrastructure roll.
            - Add Status roll.
        
9. Restart SwarmPortal service
    - Either remove the stack, or remove the SwarmPortal service and redeploy the stack.