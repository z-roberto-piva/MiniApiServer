using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.ProcessMultiplication;

namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class ProcessMultiplicationJob(ProcessMultiplicationUseCase useCase)
{
    public Task ExecuteAsync(Guid dataInId)
        => useCase.ExecuteAsync(new ProcessMultiplicationCommand(dataInId));
}
