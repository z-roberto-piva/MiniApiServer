using MiniApiServer.Domain.Common;
using MiniApiServer.Domain.Enums;

namespace MiniApiServer.Domain.Entities;

/// <summary>
/// Represents the input payload received by the API and tracked across the processing workflow.
/// </summary>
public sealed class DataIn
{
    private DataIn()
    {
        Description = string.Empty;
    }

    private DataIn(Guid id, string description, int dataA, int dataB, OperationStatus status)
    {
        Id = id;
        Description = description;
        DataA = dataA;
        DataB = dataB;
        Status = status;
    }

    /// <summary>
    /// Gets the identifier of the input row.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the human-readable description associated with the input.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Gets the first operand.
    /// </summary>
    public int DataA { get; private set; }

    /// <summary>
    /// Gets the second operand.
    /// </summary>
    public int DataB { get; private set; }

    /// <summary>
    /// Gets the current processing status.
    /// </summary>
    public OperationStatus Status { get; private set; }

    /// <summary>
    /// Creates a new input record in the initial <see cref="OperationStatus.TODO"/> state.
    /// </summary>
    public static DataIn Create(string description, int dataA, int dataB)
    {
        EnsureDescription(description);

        return new DataIn(Guid.NewGuid(), description.Trim(), dataA, dataB, OperationStatus.TODO);
    }

    /// <summary>
    /// Moves the input to the <see cref="OperationStatus.DOING"/> state.
    /// </summary>
    public void MarkAsDoing()
    {
        if (Status != OperationStatus.TODO)
        {
            throw new DomainException($"Cannot transition data input from {Status} to {OperationStatus.DOING}.");
        }

        Status = OperationStatus.DOING;
    }

    /// <summary>
    /// Moves the input to the <see cref="OperationStatus.DONE"/> state.
    /// </summary>
    public void MarkAsDone()
    {
        if (Status != OperationStatus.DOING)
        {
            throw new DomainException($"Cannot transition data input from {Status} to {OperationStatus.DONE}.");
        }

        Status = OperationStatus.DONE;
    }

    /// <summary>
    /// Updates the payload while the input is still mutable.
    /// </summary>
    public void UpdatePayload(string description, int dataA, int dataB)
    {
        if (Status == OperationStatus.DONE)
        {
            throw new DomainException("Cannot modify a completed data input.");
        }

        EnsureDescription(description);

        Description = description.Trim();
        DataA = dataA;
        DataB = dataB;
    }

    private static void EnsureDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Description is required.");
        }
    }
}
