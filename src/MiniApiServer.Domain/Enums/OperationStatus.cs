namespace MiniApiServer.Domain.Enums;

/// <summary>
/// Tracks the lifecycle of a <see cref="Entities.DataIn"/> record during background processing.
/// </summary>
public enum OperationStatus
{
    /// <summary>
    /// The input has been persisted and is waiting to be processed.
    /// </summary>
    TODO = 1,

    /// <summary>
    /// At least one background operation is currently processing the input.
    /// </summary>
    DOING = 2,

    /// <summary>
    /// All expected background operations have completed.
    /// </summary>
    DONE = 3
}
