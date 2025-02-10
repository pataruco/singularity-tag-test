using Application.Api.Enums;

namespace Application.Api.Structs;

public struct CustomerIdStruct
{
    public required string Value { get; init; }
    public required CustomerIdType Type { get; init; }
}