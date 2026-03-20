using MiniApiServer.Domain.Common;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Tests.Entities;

public sealed class StatTests
{
    [Fact]
    public void Create_ShouldStoreDailyTotals()
    {
        var date = new DateTime(2026, 3, 18, 0, 0, 0, DateTimeKind.Utc);

        var stat = Stat.Create(date, 3, 4, 3, 2, 1);

        Assert.Equal(date, stat.Date);
        Assert.Equal(3, stat.NumberOfOperations);
        Assert.Equal(4, stat.TotalOfSums);
        Assert.Equal(3, stat.TotalOfSubtractions);
        Assert.Equal(2, stat.TotalOfMultiplications);
        Assert.Equal(1, stat.TotalOfDivisions);
        Assert.NotEqual(Guid.Empty, stat.Id);
    }

    [Fact]
    public void Create_ShouldRejectNegativeTotals()
    {
        var action = () => Stat.Create(DateTime.UtcNow, 1, -1, 0, 0, 0);

        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal("Total of sums cannot be negative.", exception.Message);
    }
}
