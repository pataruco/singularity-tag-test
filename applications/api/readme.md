# Singularity GraphQL API

## Overview

This directory contains the .NET projects associated with the Singularity GraphQL API.

The API is currently attached to an in memory database which stores `User` entities, the API features CRUD operations for interacting with these entities.

### Code structure

The code for the API is organised into a number of .NET projects:
- `Application.Api` - Contains the API startup file and GraphQL associated resources.
- `Application.Api.IntegrationTest` - Contains tests for the GraphQL API.
- `Application.Core` - Contains services for business logic associated with interacting with external services/infrastructure
- `Application.Domain` - Contains domain objects/entities
- `Application.Infrastructure`- Contains classes for external infrastructure/database (e.g. `DbContext`)

## Deploying the API locally

In order to deploy the API locally, run the following command:
```
pnpm nx serve Application.Api
```

The GraphQL API will be available via the following URL `http://localhost:5095/graphql`, if you navigate to this address in a browser there will be an interactive UI for making GraphQL queries and mutations.

## Testing

For testing, this project uses NUnit, Moq and Snapshooter. In order to run the tests via the CLI, run the following command:
```
pnpm nx test Application.Api.IntegrationTest
```