using MiniApiServer.Domain.Common;
using MiniApiServer.Domain.Entities;
using MiniApiServer.Domain.Enums;

namespace MiniApiServer.Domain.Tests.Entities;

public sealed class DataInTests
{
    [Fact]
    public void Create_ShouldInitializePendingStatus()
    {
        var dataIn = DataIn.Create("sum and subtract", 10, 4);

        Assert.Equal("sum and subtract", dataIn.Description);
        Assert.Equal(10, dataIn.DataA);
        Assert.Equal(4, dataIn.DataB);
        Assert.Equal(OperationStatus.TODO, dataIn.Status);
        Assert.NotEqual(Guid.Empty, dataIn.Id);
    }

    [Fact]
    public void Create_ShouldRejectMissingDescription()
    {
        var action = () => DataIn.Create("  ", 10, 4);

        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal("Description is required.", exception.Message);
    }

    [Fact]
    public void MarkAsDoing_ShouldMoveStatusFromTodoToDoing()
    {
        var dataIn = DataIn.Create("sum and subtract", 10, 4);

        dataIn.MarkAsDoing();

        Assert.Equal(OperationStatus.DOING, dataIn.Status);
    }

    [Fact]
    public void MarkAsDoing_ShouldRejectInvalidTransition()
    {
        var dataIn = DataIn.Create("sum and subtract", 10, 4);
        dataIn.MarkAsDoing();
        dataIn.MarkAsDone();

        var action = () => dataIn.MarkAsDoing();

        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal("Cannot transition data input from DONE to DOING.", exception.Message);
    }

    [Fact]
    public void MarkAsDone_ShouldRejectSkippingDoingState()
    {
        var dataIn = DataIn.Create("sum and subtract", 10, 4);

        var action = () => dataIn.MarkAsDone();

        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal("Cannot transition data input from TODO to DONE.", exception.Message);
    }

    [Fact]
    public void UpdatePayload_ShouldRejectCompletedInput()
    {
        var dataIn = DataIn.Create("sum and subtract", 10, 4);
        dataIn.MarkAsDoing();
        dataIn.MarkAsDone();

        var action = () => dataIn.UpdatePayload("updated", 1, 2);

        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal("Cannot modify a completed data input.", exception.Message);
    }
}
