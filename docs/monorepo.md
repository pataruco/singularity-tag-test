# Monorepo tooling

## CLI tooling required for local development
The following CLI tools are required for local development:
- `docker`
- `dotnet`

## Build system
We are using [Nx](https://nx.dev/) for monorepo tooling with the [@nx-dotnet/core](https://www.nx-dotnet.com/) and [@nx/js](https://nx.dev/nx-api/js) plugins.

In the root of this monorepo we have the Nx manifest (`nx.json`) that is declaring which plugins and task configurations we can use.

Example:

```json
{
  "$schema": "./node_modules/nx/schemas/nx-schema.json",
  "defaultBase": "main",
  "plugins": ["@nx-dotnet/core", "@nx/js"]
}
```

In each application or library we have a `project.json` file that define which tasks (`serve`, `build`, `test`, etc.) can we use

Example:

```json
{
  "name": "api-app" /* Sets the name of the application */,
  "$schema": "../../../node_modules/nx/schemas/project-schema.json",
  "projectType": "application" /*Sets the type (application or library)*/,
  "sourceRoot": "applications/api/app",
  "tags": [],
  "targets": {}
}
```

## Generators

From [Nx documentation](https://nx.dev/features/generate-code)

> Generators come as part of Nx plugins and can be invoked using the nx generate command (or nx g) using the following syntax: `nx g <plugin-name>:<generator-name> [options]`.

Each Nx plugin comes with generators and executors, a generator create the scaffold and the code for an app or library.

This will create a C# application in the applications folder

```sh
pnpm nx g @nx-dotnet/core:app <NAME OF THE APPLICATION> --directory=applications/<NAME OF THE APPLICATION>
```

and this will create and link a library

```sh
pnpm nx g @nx-dotnet/core:lib <NAME OF THE LIBRARY> --directory=libraries/<NAME OF THE LIBRARY>

pnpm nx generate @nx-dotnet/core:project-reference --project=<NAME OF THE APPLICATION> --reference=<NAME OF THE LIBRARY>
```

## Executors

From [Nx documentation](https://nx.dev/concepts/executors-and-configurations)

> Executors are pre-packaged node scripts that can be used to run tasks in a consistent way.
>
> In order to use an executor, you need to install the plugin that contains the executor and then configure the executor in the project's project.json file.

To check all possible executors task run

```sh
pnpm nx show project <NAME OF THE LIBRARY | APPLICATION> --web",
```

Some examples of executors from `@nx-dotnet/core` plugin

### Build

Invokes `dotnet build` to build a project with .NET Core CLI

```sh
pnpm nx build <NAME OF THE LIBRARY | APPLICATION>
```

### serve

Invokes `dotnet watch` in combination with `dotnet build` to run a dev-server

```sh
pnpm nx serve <NAME OF THE LIBRARY | APPLICATION>
```

### test

Invokes `dotnet test` to execute unit tests via .NET Core CLI

```sh
pnpm nx test <NAME OF THE LIBRARY | APPLICATION>
```