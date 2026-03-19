namespace MiniApiServer.Api.Contracts;

public sealed record CreateDataInResponse(Guid Id, string Description, int DataA, int DataB, string Status);
