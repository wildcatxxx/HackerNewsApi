using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
public class HackerNewsService : IHackerNewsService
{
    private const string BestStoriesCacheKey = "best_story_ids";
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public HackerNewsService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<IReadOnlyList<BestStoryDto>> GetBestStoriesAsync(int n)
    {
        // Get cached best story IDs
        var storyIds = await GetBestStoryIdsAsync();
        
        // Fetch 2x the requested number to account for filtering
        var fetchCount = Math.Min(n * 2, storyIds.Count);

        // Fetch story details in parallel
        var tasks = storyIds
            .Take(fetchCount)
            .Select(GetStoryAsync);

        var stories = await Task.WhenAll(tasks);

        // Filter nulls, sort by score, and return top n
        return stories
            .Where(s => s != null)
            .OrderByDescending(s => s.Score)
            .Take(n)
            .ToList();
    }

    private async Task<List<int>> GetBestStoryIdsAsync()
    {
        if (_cache.TryGetValue(BestStoriesCacheKey, out List<int> cached))
            return cached;

        var response = await _httpClient.GetStringAsync(
            "https://hacker-news.firebaseio.com/v0/beststories.json");

        var ids = JsonSerializer.Deserialize<List<int>>(response);

        _cache.Set(BestStoriesCacheKey, ids, TimeSpan.FromMinutes(5));
        return ids;
    }

    private async Task<BestStoryDto?> GetStoryAsync(int id)
    {
        var cacheKey = $"story_{id}";
        if (_cache.TryGetValue(cacheKey, out BestStoryDto cached))
            return cached;

        var response = await _httpClient.GetAsync(
            $"https://hacker-news.firebaseio.com/v0/item/{id}.json");

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        var item = JsonSerializer.Deserialize<HackerNewsItem>(json);

        if (item == null || string.IsNullOrEmpty(item.title))
            return null;

        var dto = new BestStoryDto
        {
            Title = item.title,
            Uri = item.url,
            PostedBy = item.by,
            Time = DateTimeOffset.FromUnixTimeSeconds(item.time),
            Score = item.score,
            CommentCount = item.descendants
        };

        _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(10));
        return dto;
    }
}
