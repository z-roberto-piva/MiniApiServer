using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.ProcessSum;

namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class ProcessSumJob(ProcessSumUseCase useCase)
{
    public Task ExecuteAsync(Guid dataInId)
        => useCase.ExecuteAsync(new ProcessSumCommand(dataInId));
}
