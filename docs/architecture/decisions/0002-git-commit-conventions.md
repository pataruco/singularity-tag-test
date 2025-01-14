# 2. git-commit-conventions

Date: 2025-01-14

## Status

Accepted

## Context

Ccommit messages may vary wildly in format and clarity, making it difficult to:

- Understand the nature of changes at a glance
- Automate release notes and changelogs
- Efficiently search and filter commits

## Decision

We will adopt [**Conventional Commits**](https://www.conventionalcommits.org/en/v1.0.0/) as the standard for all commits within this project.

Conventional Commits is a lightweight convention on top of the existing commit message specification. It provides an easy-to-parse format for automated tools.

### Examples

- feat: add new feature (e.g., feat: allow provided config object to be merged into the default config)
- fix: fix a bug (e.g., fix: address occasional rendering issue in long lists)
- docs: add documentation (e.g., docs: update README.md with new command usage)
- refactor: code change that neither fixes a bug nor adds a feature (e.g., refactor: extract component A into a separate file)
- perf: improve performance (e.g., perf: cache API responses in memory)
- test: add missing tests (e.g., test: add unit tests for the new validation function)
- ci: add changes to ci/cd (e.g., ci: add new linter)

## Consequences

### Benefits:

- Improved code readability and maintainability: Consistent and informative commit messages make it easier to understand the history of the project.
- Automated release notes and changelogs: Tools can automatically generate release notes based on commit messages, saving time and reducing errors.
- Simplified code search and filtering: Easily find specific types of changes (e.g., all bug fixes, all breaking changes) using tools that support Conventional Commits.
- Improved collaboration: Clear and consistent communication between developers.

### Drawbacks:

- Initial learning curve: Developers will need to learn the new format.
- Enforcing compliance: May require some initial effort to ensure consistent adherence to the convention.

### Mitigations:

- Provide clear guidelines and examples to all developers.
- Regularly review and enforce the use of Conventional Commits.
