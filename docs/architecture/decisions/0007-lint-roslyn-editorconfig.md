# 7. lint-roslyn-editorconfig

Date: 2025-02-05

## Status

Accepted

## Context

The platform engineering team wants to understand the options for linting code in C# using .NET and ASP.NET Core. The older .NET Framework will only run on Windows machines and will not be considered in this ADR, as its challenges, call for a targeted solution.

There is a rich ecosystem of .NET linting tools that we may want to try on this project: SonarQube for IDE (formerly SonarLint), ReSharper, and the Roslyn compiler, which comes with default analysers. Our engineering team uses three IDEs: Rider, Visual Studio Code (VS Code), and Visual Studio. The ideal solution will provide a mechanism for sharing settings across all three.

## Decision

Use Roslyn for linting rules and editor config to share settings.

Roslyn is the default C# compiler for .NET and can lint code style on the fly. It is compatible with [editorconfig](https://editorconfig.org/), a project that aims to allow for "consistent coding styles for multiple developers working on the same project across various editors and IDEs."

Using a `.editorconfig` file, you can set the [severity](https://learn.microsoft.com/en-us/visualstudio/code-quality/use-roslyn-analyzers?view=vs-2022) for individual rules, groups of rules and globally.

## Consequences

An advantage of using `.editorconfig` is that if we decide we want additional linting tools like ReSharper, they are also compatible with `.editorconfig`.

The Visual Studio and Rider IDEs come with built-in support for `.editorconfig,` while VS Code users will need to install a plugin.

Given this decision, we should attempt to move all editor configurations into `.editorconfig`.
