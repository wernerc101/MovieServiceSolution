using Microsoft.AspNetCore.Mvc;
using MovieService.Api.DTO;
using MovieService.Api.Interfaces;
using System.Threading.Tasks;

namespace MovieService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetail>> GetMovie(string id)
        {
            var movieDetail = await _movieService.GetMovieDetailAsync(id);
            if (movieDetail == null)
            {
                return NotFound();
            }
            return Ok(movieDetail);
        }

        [HttpPost("search")]
        public async Task<ActionResult<MovieSearchResponse>> SearchMovies([FromBody] MovieSearchRequest request)
        {
            var response = await _movieService.SearchMoviesAsync(request);
            return Ok(response);
        }
    }
}