
FROM node:18 as build-ui
WORKDIR /app
COPY SwarmPortal.FrontEnd/package*.json /app/
RUN npm ci
COPY SwarmPortal.FrontEnd /app/
ARG configuration=production
RUN npm run build -- --output-path=./dist/out --configuration $configuration

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-api

WORKDIR /app
COPY . ./
WORKDIR /app/SwarmPortal.API
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-api /app/SwarmPortal.API/out .
RUN mkdir -p /app/clientapp
COPY --from=build-ui /app/dist/out/ ./wwwroot/
RUN mkdir -p /app/persist
ENTRYPOINT ["/app/SwarmPortal.API"]