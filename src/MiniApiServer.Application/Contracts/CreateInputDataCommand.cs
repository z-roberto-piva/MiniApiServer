namespace MiniApiServer.Application.Contracts;

public sealed record CreateInputDataCommand(string Description, int DataA, int DataB);
