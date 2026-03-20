using System.ComponentModel.DataAnnotations;

namespace MiniApiServer.Api.Contracts;

/// <summary>
/// HTTP payload used to create a new input row.
/// </summary>
public sealed class CreateDataInRequest
{
    /// <summary>
    /// Functional description associated with the input.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    [MaxLength(512)]
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// First operand.
    /// </summary>
    public int DataA { get; init; }

    /// <summary>
    /// Second operand.
    /// </summary>
    public int DataB { get; init; }
}
