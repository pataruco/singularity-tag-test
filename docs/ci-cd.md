# Continous integration | Continous deployment (CI/CD)

For CI/CD are using [Nx](https://nx.dev/getting-started/intro) as monorepo tooling in tandem with GitHub Actions

For GitHub Actions runners, we have three types:

- `ubuntu-latest`: which runs as shared GitHub runner with the Ubuntu distro
- `self-hosted-app`: a self-hosted runner, generally used by the app team
- `self-hosted-cdp`: which is a self-hosted runner, normally used by the CDP team, you can check the base image on [`docs/self-hosted-runners/cdp-team.dockerfile`](docs/self-hosted-runners/cdp-team.dockerfile)

You can check the list of available self-hosted runners [here](https://github.com/Just-Universe/universe-customer-data-platform/settings/actions/runners)

## Continous integration

We created a GitHub two GitHub workflows.

### `pr`

It runs test and lint on files affected on the pull request.

### `integration`

I run end-to-end tests on the libraries and applications affected by merge to the `main` branch.

## Continous deployment

We created one file

### `build-and-push`

Using `self-hosted-cdp`eds to be build and if it's so it will parallelised each build image and push Azure container registry

```mermaid
flowchart TD
    subgraph "Job: build-and-push (matrix strategy)"
        E1[Checkout code] --> E2[Prepare monorepo tooling]
        E2 --> E3[Build application]
        E3 --> E4[Login to Azure]
        E4 --> E5[Run ACR task to build and push image]
    end
    subgraph "Job: get-applications-to-build"
        C1[Checkout code] --> C2[Prepare monorepo tooling]
        C2 --> C3[Get affected files]
        C3 --> C4[Identify affected applications]
        C4 --> C5[Output applications list]
    end

    A[Push to main branch] -->|Paths match workflow criteria| B[Workflow Triggered]
    B --> C[Job: get-applications-to-build]
    C --> D{Applications to build?}
    D -->|Yes| E[Job: build-and-push]
    D -->|No| F[Workflow ends]


    style A fill:#d0e0ff
    style F fill:#ffcccc
    style E5 fill:#d5f5d5
```
