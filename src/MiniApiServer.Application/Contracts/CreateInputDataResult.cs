using MiniApiServer.Domain.Enums;

namespace MiniApiServer.Application.Contracts;

/// <summary>
/// Result returned after a new input has been created and scheduled.
/// </summary>
/// <param name="DataInId">Identifier of the created input row.</param>
/// <param name="Description">Stored description.</param>
/// <param name="DataA">Stored first operand.</param>
/// <param name="DataB">Stored second operand.</param>
/// <param name="Status">Initial processing status.</param>
public sealed record CreateInputDataResult(Guid DataInId, string Description, int DataA, int DataB, OperationStatus Status);
