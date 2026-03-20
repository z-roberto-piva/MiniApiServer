namespace MiniApiServer.Application.Abstractions.Jobs;

/// <summary>
/// Resolves the configured category assigned to a specific background job type.
/// </summary>
public interface IBackgroundJobCategoryResolver
{
    /// <summary>
    /// Returns the category to use for the requested job type.
    /// </summary>
    BackgroundJobCategory ResolveCategory(BackgroundJobType jobType);
}
