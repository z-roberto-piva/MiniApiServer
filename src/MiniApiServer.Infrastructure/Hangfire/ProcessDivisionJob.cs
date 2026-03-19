using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.ProcessDivision;

namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class ProcessDivisionJob(ProcessDivisionUseCase useCase)
{
    public Task ExecuteAsync(Guid dataInId)
        => useCase.ExecuteAsync(new ProcessDivisionCommand(dataInId));
}
