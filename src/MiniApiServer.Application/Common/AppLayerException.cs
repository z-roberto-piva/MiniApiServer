namespace MiniApiServer.Application.Common;

/// <summary>
/// Exception raised when an application use case cannot complete successfully.
/// </summary>
public sealed class AppLayerException(string message) : Exception(message);
