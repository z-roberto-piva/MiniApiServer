namespace MiniApiServer.Application.Contracts;

/// <summary>
/// Command used to create a new input record and schedule its background jobs.
/// </summary>
/// <param name="Description">Functional description associated with the input.</param>
/// <param name="DataA">First operand.</param>
/// <param name="DataB">Second operand.</param>
public sealed record CreateInputDataCommand(string Description, int DataA, int DataB);
