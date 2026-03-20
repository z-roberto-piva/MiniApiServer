using MiniApiServer.Domain.Common;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Tests.Entities;

public sealed class DataSummTests
{
    [Fact]
    public void Create_ShouldStoreResult()
    {
        var inputId = Guid.NewGuid();

        var dataSumm = DataSumm.Create(inputId, "sum", 14);

        Assert.Equal(inputId, dataSumm.DataInId);
        Assert.Equal("sum", dataSumm.Description);
        Assert.Equal(14, dataSumm.Result);
        Assert.NotEqual(Guid.Empty, dataSumm.Id);
    }

    [Fact]
    public void Create_ShouldRejectMissingInputId()
    {
        var action = () => DataSumm.Create(Guid.Empty, "sum", 14);

        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal("Data input id is required.", exception.Message);
    }
}
