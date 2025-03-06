# 10. grpc-proto-library

Date: 2025-03-05

## Status

Accepted

## Context

At least one of the microservices will communicate using gRPC, which relies on protobuf (.proto) files to define service contracts.
We want to ensure that the protobuf files are accessible across multiple projects but avoid unnecessary duplication of code,
so that it is more maintainable.

## Decision
Each microservice will have a library to package the protobuf definitions so that other services can reference them as needed.
There will be a dedicated library for the microservice's protobufs
Since we use a monorepo, we will create a shared class library to house the .proto files. This allows services to reference the compiled protobuf files via project references, instead of independently compiling the protobuf files.


## Consequences

- **Single Compilation Per Service**: Each service's protobuf files will only need to be compiled once, reducing duplication and ensuring consistency.
- **Avoiding Conflicts**: By referencing compiled protobuf libraries instead of recompiling, we prevent multiple definitions of the same generated code, which could cause build issues.
- **Encapsulation and Maintainability**: Each service owns and maintains its own protobuf definitions, making updates and versioning more manageable.
- **No External Exposure**: Since gRPC endpoints are internal to the CDP, this approach ensures that protobuf definitions are only shared among services without exposing them externally.
- **Improved Dependency Management**: By structuring protobuf definitions into libraries, dependency updates are more controlled, reducing unexpected breaking changes.
