# Singularity (name still in discussion)

## What is it?

It is a monorepo for all the services that will sustain the Universe platform.

### How it is divided?

- [Documentation](/docs/readme.md)

## Installation

We are using [Nx](https://nx.dev/) which is a set of JavaScript libaries for monorepo tooling.

You must have a Node version equal or greater than `22.13.0` with `pnmp` as package manager

### Node

We are assuming you are using [NVM](https://github.com/nvm-sh/nvm) as node version manager

```sh
nvm install lts/jod
```

### pnpm

Once the correct version of Node is installed, please add pnpm as your package manager

```sh
corepack enable pnpm
corepack use pnpm@latest
```
