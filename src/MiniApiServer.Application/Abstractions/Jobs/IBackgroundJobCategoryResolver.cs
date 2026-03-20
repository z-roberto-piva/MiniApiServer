namespace MiniApiServer.Application.Abstractions.Jobs;

public interface IBackgroundJobCategoryResolver
{
    BackgroundJobCategory ResolveCategory(BackgroundJobType jobType);
}
