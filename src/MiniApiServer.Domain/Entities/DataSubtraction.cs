using MiniApiServer.Domain.Common;

namespace MiniApiServer.Domain.Entities;

public sealed class DataSubtraction
{
    private DataSubtraction(Guid id, Guid dataInId, string description, int result)
    {
        Id = id;
        DataInId = dataInId;
        Description = description;
        Result = result;
    }

    public Guid Id { get; }

    public Guid DataInId { get; }

    public string Description { get; }

    public int Result { get; }

    public static DataSubtraction Create(Guid dataInId, string description, int result)
    {
        if (dataInId == Guid.Empty)
        {
            throw new DomainException("Data input id is required.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Description is required.");
        }

        return new DataSubtraction(Guid.NewGuid(), dataInId, description.Trim(), result);
    }
}
