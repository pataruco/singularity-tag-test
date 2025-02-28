# Auth0 Post-Login Trigger

## Overview

The code within this project is for the Auth0 Post-Login trigger which is used for creating a Contact entity in Dynamics Dataverse using the `user_id` and `email` fields on the Auth0 user entity.

## Deployment

The Auth0 trigger is deployed to a post-login action using Terraform which is hosted in the [universe-infra](https://github.com/Just-Universe/universe-infra) repository. 

## Unit testing

The package uses Jest for unit testing, these tests can be executed using the following commands:
- Running unit tests: `nx test auth0-post-login-trigger`
- Collecting unit test coverage: `nx test:coverage auth0-post-login-trigger`