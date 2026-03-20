namespace MiniApiServer.Application.Contracts;

/// <summary>
/// Command that requests subtraction processing for a specific input.
/// </summary>
/// <param name="DataInId">Identifier of the source input.</param>
public sealed record ProcessSubtractionCommand(Guid DataInId);
