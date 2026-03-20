using MiniApiServer.Domain.Common;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Tests.Entities;

public sealed class DataSubtractionTests
{
    [Fact]
    public void Create_ShouldStoreResult()
    {
        var inputId = Guid.NewGuid();

        var dataSubtraction = DataSubtraction.Create(inputId, "subtraction", 6);

        Assert.Equal(inputId, dataSubtraction.DataInId);
        Assert.Equal("subtraction", dataSubtraction.Description);
        Assert.Equal(6, dataSubtraction.Result);
        Assert.NotEqual(Guid.Empty, dataSubtraction.Id);
    }

    [Fact]
    public void Create_ShouldRejectMissingDescription()
    {
        var action = () => DataSubtraction.Create(Guid.NewGuid(), string.Empty, 6);

        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal("Description is required.", exception.Message);
    }
}
