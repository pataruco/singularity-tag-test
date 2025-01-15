# 3. pull-request-strategy

Date: 2025-01-15

## Status

Accepted

## Context

We need a consistent and reliable process for merging code changes into our main branch to ensure code quality, maintainability, and stability.

## Decision

We will adopt the following pull request strategy:

- Template: All pull requests must be created using a standardized template. This template will include sections for:

```md
## Tickets

- resolve #

## Description

- What this PR does
- How to run it locally
- How to manually test it

## Checklist

- [ ] Tests
- [ ] Documentation

## GIF

![]()
```

- Review: At least one code review is required from a qualified engineer before a pull request can be merged.

- Main Branch Protection: The main branch will be protected with the following safeguards:
- Required Reviews: No merges are allowed without the required number of approvals.
- Status Checks: All required CI/CD pipelines (e.g., unit tests, integration tests, security scans) must pass successfully before a merge can be performed.

## Consequences

- Potential for delays if reviewers are unavailable or if there are issues with the CI/CD pipelines.
- May slightly increase the time required to merge code changes.
- Improved code quality and maintainability due to increased code review and automated testing.
- Reduced risk of introducing bugs into the main branch.
- More consistent and predictable code merging process.
