using Microsoft.AspNetCore.Mvc;

/// <summary>
/// API controller that exposes endpoints to retrieve Hacker News "best" stories.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BestStoriesController : ControllerBase
{
    // Service used to fetch stories from Hacker News
    private readonly IHackerNewsService _service;

    public BestStoriesController(IHackerNewsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int n = 10)
    {
        // Validate the requested number of stories
        if (n <= 0 || n > 100)
            return BadRequest("n must be from 1 and 100.");

        // Fetch stories from the service and return them to the caller
        var stories = await _service.GetBestStoriesAsync(n);
        return Ok(stories);
    }
}
