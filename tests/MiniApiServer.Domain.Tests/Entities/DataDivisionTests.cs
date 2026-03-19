using MiniApiServer.Domain.Common;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Tests.Entities;

public sealed class DataDivisionTests
{
    [Fact]
    public void Create_ShouldStoreResult()
    {
        var inputId = Guid.NewGuid();

        var dataDivision = DataDivision.Create(inputId, "division", 4);

        Assert.Equal(inputId, dataDivision.DataInId);
        Assert.Equal("division", dataDivision.Description);
        Assert.Equal(4, dataDivision.Result);
        Assert.NotEqual(Guid.Empty, dataDivision.Id);
    }

    [Fact]
    public void Create_ShouldRejectMissingInputId()
    {
        var action = () => DataDivision.Create(Guid.Empty, "division", 4);

        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal("Data input id is required.", exception.Message);
    }
}
