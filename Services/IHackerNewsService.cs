public interface IHackerNewsService
{
    Task<IReadOnlyList<BestStoryDto>> GetBestStoriesAsync(int n);
}
