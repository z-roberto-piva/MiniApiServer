namespace MiniApiServer.Api.Contracts;

/// <summary>
/// HTTP response returned after a new input row has been created.
/// </summary>
/// <param name="Id">Identifier of the created input row.</param>
/// <param name="Description">Stored description.</param>
/// <param name="DataA">Stored first operand.</param>
/// <param name="DataB">Stored second operand.</param>
/// <param name="Status">Current processing status.</param>
public sealed record CreateDataInResponse(Guid Id, string Description, int DataA, int DataB, string Status);
