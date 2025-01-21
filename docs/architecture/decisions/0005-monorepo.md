# 5. monorepo

Date: 2025-01-21

## Status

Accepted

## Context

We want to develop a suite of microservices to power the Universe app. We want to have the flexibility to manage everything as code from the same version control, where it is easy to share code, easily manage dependencies, and consistent development and testing practices.

## Decision

We propose a monorepo architecture for our microservices. This means all microservices and shared libraries will be housed within a single repository.

### How did we reach this decision

We created two different proofs of concept, one using [Turborepo][turborepo] and the other [Nx][nx] as monorepo tooling. We followed the recipes on how to plug in it and how to run it. We created two different pull requests (PR) to showcase the work; please check out the [Turborepo POC PR](https://github.com/redbadger/singularity/pull/3) and the [Nx POC PR](https://github.com/redbadger/singularity/pull/6); then the technical team met and discussed each approach.

We agreed to use [Nx][nx] as monorepo tooling because is:

- Language agnostic: Nx is designed to support various programming languages and frameworks, including JavaScript/TypeScript, C# and more. This flexibility is crucial for our project, which may involve using different languages for different microservices. Turborepo, while powerful, primarily focuses on JavaScript and TypeScript.
- Distributed task execution: Distribute tasks across the network for faster build and test times.
- Remote caching: Cache builds artefacts remotely to improve build performance further.
- Fine-grained dependency Management: Control dependencies between projects within the monorepo.
- Powerful CLI: A comprehensive command-line interface for managing tasks, running tests, and building projects.

### Potential drawbacks of monorepo using Nx

Nx has a slightly steeper learning curve than Turborepo, which has a simpler and more streamlined approach.

## Consequences

### Cons

- The monorepo will become more significant, affecting build times and repository management.
- While tools can help manage complexity, careful planning and organisation are essential to maintain a well-structured and maintainable monorepo.
- Developers may need time to adapt to working in a monorepo environment.

### Pros

- Sharing standard code and libraries between services becomes significantly easier.
- Tools can automatically detect and resolve dependencies between services within the monorepo.
- Building and testing changes can be more efficient as the entire system can be built and tested as a unit.
- Developers can easily explore and contribute to other microservices within the monorepo.

[turborepo]: https://turbo.build/
[nx]: https://nx.dev/
