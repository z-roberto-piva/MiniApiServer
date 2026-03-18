using MiniApiServer.Domain.Common;

namespace MiniApiServer.Domain.Entities;

public sealed class Stat
{
    private Stat()
    {
    }

    private Stat(Guid id, DateTime date, int numberOfOperations, int totalOfSums, int totalOfSubtractions)
    {
        Id = id;
        Date = date;
        NumberOfOperations = numberOfOperations;
        TotalOfSums = totalOfSums;
        TotalOfSubtractions = totalOfSubtractions;
    }

    public Guid Id { get; private set; }

    public DateTime Date { get; private set; }

    public int NumberOfOperations { get; private set; }

    public int TotalOfSums { get; private set; }

    public int TotalOfSubtractions { get; private set; }

    public static Stat Create(DateTime date, int numberOfOperations, int totalOfSums, int totalOfSubtractions)
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

        return new Stat(Guid.NewGuid(), date, numberOfOperations, totalOfSums, totalOfSubtractions);
    }
}
