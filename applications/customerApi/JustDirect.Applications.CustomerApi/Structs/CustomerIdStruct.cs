using JustDirect.Applications.CustomerApi.Enums;

namespace JustDirect.Applications.CustomerApi.Structs;

public struct CustomerIdStruct
{
    public required string Value { get; init; }
    public required CustomerIdType Type { get; init; }
}