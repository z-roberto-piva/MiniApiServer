using MiniApiServer.Domain.Common;
using MiniApiServer.Domain.Enums;

namespace MiniApiServer.Domain.Entities;

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

    public Guid Id { get; private set; }

    public string Description { get; private set; }

    public int DataA { get; private set; }

    public int DataB { get; private set; }

    public OperationStatus Status { get; private set; }

    public static DataIn Create(string description, int dataA, int dataB)
    {
        EnsureDescription(description);

        return new DataIn(Guid.NewGuid(), description.Trim(), dataA, dataB, OperationStatus.TODO);
    }

    public void MarkAsDoing()
    {
        if (Status != OperationStatus.TODO)
        {
            throw new DomainException($"Cannot transition data input from {Status} to {OperationStatus.DOING}.");
        }

        Status = OperationStatus.DOING;
    }

    public void MarkAsDone()
    {
        if (Status != OperationStatus.DOING)
        {
            throw new DomainException($"Cannot transition data input from {Status} to {OperationStatus.DONE}.");
        }

        Status = OperationStatus.DONE;
    }

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
