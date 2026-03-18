using MiniApiServer.Domain.Common;

namespace MiniApiServer.Domain.Entities;

public sealed class DataSumm
{
    private DataSumm()
    {
        Description = string.Empty;
    }

    private DataSumm(Guid id, Guid dataInId, string description, int result)
    {
        Id = id;
        DataInId = dataInId;
        Description = description;
        Result = result;
    }

    public Guid Id { get; private set; }

    public Guid DataInId { get; private set; }

    public string Description { get; private set; }

    public int Result { get; private set; }

    public static DataSumm Create(Guid dataInId, string description, int result)
    {
        if (dataInId == Guid.Empty)
        {
            throw new DomainException("Data input id is required.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Description is required.");
        }

        return new DataSumm(Guid.NewGuid(), dataInId, description.Trim(), result);
    }
}
