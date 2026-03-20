using MiniApiServer.Domain.Common;

namespace MiniApiServer.Domain.Entities;

/// <summary>
/// Stores the result of the division operation generated from a <see cref="DataIn"/> record.
/// </summary>
public sealed class DataDivision
{
    private DataDivision()
    {
        Description = string.Empty;
    }

    private DataDivision(Guid id, Guid dataInId, string description, int result)
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
    /// Gets the computed division.
    /// </summary>
    public int Result { get; private set; }

    /// <summary>
    /// Creates a new division result.
    /// </summary>
    public static DataDivision Create(Guid dataInId, string description, int result)
    {
        if (dataInId == Guid.Empty)
        {
            throw new DomainException("Data input id is required.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Description is required.");
        }

        return new DataDivision(Guid.NewGuid(), dataInId, description.Trim(), result);
    }
}
