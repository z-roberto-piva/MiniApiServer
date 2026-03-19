using System.ComponentModel.DataAnnotations;

namespace MiniApiServer.Api.Contracts;

public sealed class CreateDataInRequest
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(512)]
    public string Description { get; init; } = string.Empty;

    public int DataA { get; init; }

    public int DataB { get; init; }
}
