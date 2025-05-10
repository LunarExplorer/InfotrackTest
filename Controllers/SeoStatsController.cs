using InfotrackTest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfotrackTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeoStatsController : ControllerBase
    {
        private readonly ISeoStatsProvider _seoStatsProvider;

        public SeoStatsController(ISeoStatsProvider seoStatsProvider)
        {
            _seoStatsProvider = seoStatsProvider;
        }

        [HttpGet("positions")]
        public async Task<IActionResult> GetPositions([FromQuery] string[] keywords, [FromQuery] string subjectUrl)
        {
            if (keywords == null || keywords.Length == 0 || string.IsNullOrEmpty(subjectUrl))
            {
                return BadRequest("Keywords and subjectUrl are required.");
            }

            var seoStats = await _seoStatsProvider.GetSeoStats(keywords, subjectUrl);
            return Ok(seoStats);
        }
    }
}
