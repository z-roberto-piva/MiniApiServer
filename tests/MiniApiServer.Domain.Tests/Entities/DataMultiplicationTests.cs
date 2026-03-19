using MiniApiServer.Domain.Common;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Tests.Entities;

public sealed class DataMultiplicationTests
{
    [Fact]
    public void Create_ShouldStoreResult()
    {
        var inputId = Guid.NewGuid();

        var dataMultiplication = DataMultiplication.Create(inputId, "multiplication", 20);

        Assert.Equal(inputId, dataMultiplication.DataInId);
        Assert.Equal("multiplication", dataMultiplication.Description);
        Assert.Equal(20, dataMultiplication.Result);
        Assert.NotEqual(Guid.Empty, dataMultiplication.Id);
    }

    [Fact]
    public void Create_ShouldRejectMissingDescription()
    {
        var action = () => DataMultiplication.Create(Guid.NewGuid(), string.Empty, 20);

        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal("Description is required.", exception.Message);
    }
}
