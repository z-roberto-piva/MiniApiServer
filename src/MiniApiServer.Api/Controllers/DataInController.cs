using Microsoft.AspNetCore.Mvc;
using MiniApiServer.Api.Contracts;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.CreateInputData;

namespace MiniApiServer.Api.Controllers;

/// <summary>
/// Exposes the endpoint used to create new input rows for background processing.
/// </summary>
[ApiController]
[Route("api/data-in")]
public sealed class DataInController(CreateInputDataUseCase useCase) : ControllerBase
{
    /// <summary>
    /// Creates a new input row and schedules the related background jobs.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateDataInResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateDataInResponse>> CreateAsync(
        [FromBody] CreateDataInRequest request,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(
            new CreateInputDataCommand(request.Description, request.DataA, request.DataB),
            cancellationToken);

        var response = new CreateDataInResponse(
            result.DataInId,
            result.Description,
            result.DataA,
            result.DataB,
            result.Status.ToString());

        return Created($"/api/data-in/{response.Id}", response);
    }
}
