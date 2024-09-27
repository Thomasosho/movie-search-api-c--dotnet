using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MovieSearchApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly SearchHistoryService _searchHistoryService;
        private const string ApiKey = "ea64bce8";
        private const string BaseUrl = "http://www.omdbapi.com/";

        public MoviesController(IHttpClientFactory clientFactory, SearchHistoryService searchHistoryService)
        {
            _clientFactory = clientFactory;
            _searchHistoryService = searchHistoryService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string title, int page = 1)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{BaseUrl}?apikey={ApiKey}&s={title}&page={page}");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error fetching data from OMDB API");
            }

            var content = await response.Content.ReadAsStringAsync();
            _searchHistoryService.AddSearch(title);

            return Ok(content);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(string id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{BaseUrl}?apikey={ApiKey}&i={id}&plot=full");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error fetching data from OMDB API");
            }

            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        [HttpGet("history")]
        public IActionResult GetSearchHistory()
        {
            return Ok(_searchHistoryService.GetSearchHistory());
        }
    }
}