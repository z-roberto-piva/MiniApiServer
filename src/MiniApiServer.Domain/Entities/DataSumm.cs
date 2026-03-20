using MiniApiServer.Domain.Common;

namespace MiniApiServer.Domain.Entities;

/// <summary>
/// Stores the result of the sum operation generated from a <see cref="DataIn"/> record.
/// </summary>
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

    /// <summary>
    /// Gets the identifier of the persisted result.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the source input identifier.
    /// </summary>
    public Guid DataInId { get; private set; }

    /// <summary>
    /// Gets the copied description of the source input.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Gets the computed sum.
    /// </summary>
    public int Result { get; private set; }

    /// <summary>
    /// Creates a new sum result.
    /// </summary>
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
