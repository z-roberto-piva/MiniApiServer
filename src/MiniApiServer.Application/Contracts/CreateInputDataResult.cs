using MiniApiServer.Domain.Enums;

namespace MiniApiServer.Application.Contracts;

public sealed record CreateInputDataResult(Guid DataInId, string Description, int DataA, int DataB, OperationStatus Status);
