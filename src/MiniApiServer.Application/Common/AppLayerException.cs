namespace MiniApiServer.Application.Common;

public sealed class AppLayerException(string message) : Exception(message);
