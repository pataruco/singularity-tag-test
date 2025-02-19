# Singularity GraphQL API

## Overview

This directory contains the .NET projects associated with the Customer Graph API which is a GraphQL API.

The API is attached to Microsoft Dataverse for interacting with `Contact` entities and returning `Customer` entities.

### Code structure

The code for the API is organised into a number of .NET projects:

- `Application.Api` - Contains the API startup file and GraphQL associated resources.
- `Application.Api.IntegrationTest` - Contains tests for the GraphQL API.
- `Application.Core` - Contains services for business logic associated with interacting with external services/infrastructure
- `Application.Domain` - Contains domain objects/entities
- `Application.Infrastructure`- Contains classes for external infrastructure/database (e.g. `DbContext`)

## AppSettings

In order for the API to connect to the Dataverse CRM, you will need to populate the `DynamicsClient` options section of the `Applications.Api/appsettings.json` file with valid values. We intend for these values to be stored in key vault at a later date for ease of use.

## Deploying the API locally

In order to deploy the API locally, run the following command:

```
pnpm nx serve Application.Api
```

The GraphQL API will be available via the following URL http://localhost:5095/graphql, if you navigate to this address in a browser there will be an interactive UI for making GraphQL queries and mutations.

## Deploying API in a container

First build the container image using the following commmand

```
pnpm nx build-container CustomerGraphApi
```

Then serve the container using the following command:

```sh
pnpm nx serve-container CustomerGraphApi
```

The API should be available at http://localhost:8080/graphql

## Testing

For testing, this project uses NUnit, Moq and Snapshooter. In order to run the tests via the CLI, run the following command from the root directory where the `singularity.sln` exists:

```sh
dotnet test
```
