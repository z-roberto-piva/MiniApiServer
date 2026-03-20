using MiniApiServer.Domain.Common;

namespace MiniApiServer.Domain.Entities;

/// <summary>
/// Represents the daily aggregate snapshot of completed operations.
/// </summary>
public sealed class Stat
{
    private Stat()
    {
    }

    private Stat(
        Guid id,
        DateTime date,
        int numberOfOperations,
        int totalOfSums,
        int totalOfSubtractions,
        int totalOfMultiplications,
        int totalOfDivisions)
    {
        Id = id;
        Date = date;
        NumberOfOperations = numberOfOperations;
        TotalOfSums = totalOfSums;
        TotalOfSubtractions = totalOfSubtractions;
        TotalOfMultiplications = totalOfMultiplications;
        TotalOfDivisions = totalOfDivisions;
    }

    /// <summary>
    /// Gets the identifier of the statistic row.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the day represented by the snapshot.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Gets the total number of processed operations.
    /// </summary>
    public int NumberOfOperations { get; private set; }

    /// <summary>
    /// Gets the aggregate sum of all additions.
    /// </summary>
    public int TotalOfSums { get; private set; }

    /// <summary>
    /// Gets the aggregate sum of all subtractions.
    /// </summary>
    public int TotalOfSubtractions { get; private set; }

    /// <summary>
    /// Gets the aggregate sum of all multiplications.
    /// </summary>
    public int TotalOfMultiplications { get; private set; }

    /// <summary>
    /// Gets the aggregate sum of all divisions.
    /// </summary>
    public int TotalOfDivisions { get; private set; }

    /// <summary>
    /// Creates a new daily statistics snapshot.
    /// </summary>
    public static Stat Create(
        DateTime date,
        int numberOfOperations,
        int totalOfSums,
        int totalOfSubtractions,
        int totalOfMultiplications,
        int totalOfDivisions)
    {
        if (numberOfOperations < 0)
        {
            throw new DomainException("Number of operations cannot be negative.");
        }

        if (totalOfSums < 0)
        {
            throw new DomainException("Total of sums cannot be negative.");
        }

        if (totalOfSubtractions < 0)
        {
            throw new DomainException("Total of subtractions cannot be negative.");
        }

        if (totalOfMultiplications < 0)
        {
            throw new DomainException("Total of multiplications cannot be negative.");
        }

        if (totalOfDivisions < 0)
        {
            throw new DomainException("Total of divisions cannot be negative.");
        }

        return new Stat(
            Guid.NewGuid(),
            date,
            numberOfOperations,
            totalOfSums,
            totalOfSubtractions,
            totalOfMultiplications,
            totalOfDivisions);
    }
}
