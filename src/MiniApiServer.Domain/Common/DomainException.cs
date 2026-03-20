namespace MiniApiServer.Domain.Common;

/// <summary>
/// Exception raised when a domain invariant is violated.
/// </summary>
public sealed class DomainException(string message) : Exception(message);
