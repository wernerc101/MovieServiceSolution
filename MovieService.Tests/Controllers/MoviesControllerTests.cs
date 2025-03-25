using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieService.Api.Controllers;
using MovieService.Api.DTO;
using MovieService.Api.Interfaces;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Threading.Tasks;

namespace MovieService.Tests.Controllers
{
  [TestFixture]
  public class MoviesControllerTests
  {
    private Mock<IMovieService> _mockMovieService;
    private MoviesController _controller;

    [SetUp]
    public void Setup()
    {
      _mockMovieService = new Mock<IMovieService>();
      _controller = new MoviesController(_mockMovieService.Object);
    }

    [Test]
    public async Task GetMovie_WithExistingId_ReturnsOkResult()
    {
      // Arrange
      string movieId = "tt0111161"; // The Shawshank Redemption
      _mockMovieService.Setup(service => service.GetMovieDetailsAsync(movieId))
          .ReturnsAsync(new MovieDetail { ImdbID = movieId, Title = "The Shawshank Redemption", Year = "1994" });

      // Act
      var result = await _controller.GetMovie(movieId);

      // Assert
      ClassicAssert.IsInstanceOf<OkObjectResult>(result.Result);
      var okResult = (OkObjectResult)result.Result;
      ClassicAssert.IsInstanceOf<MovieDetail>(okResult.Value);
      var returnValue = (MovieDetail)okResult.Value;
      ClassicAssert.AreEqual(movieId, returnValue.ImdbID);
      ClassicAssert.AreEqual("The Shawshank Redemption", returnValue.Title);
    }

    [Test]
    public async Task GetMovie_WithNonExistingId_ReturnsNotFound()
    {
      // Arrange
      string movieId = "nonExistingId";
      _mockMovieService.Setup(service => service.GetMovieDetailsAsync(movieId))
          .ReturnsAsync((MovieDetail)null);

      // Act
      var result = await _controller.GetMovie(movieId);

      // Assert
      ClassicAssert.IsInstanceOf<NotFoundResult>(result.Result);
    }

    [Test]
    public async Task SearchMovies_WithValidRequest_ReturnsOkResult()
    {
      // Arrange
      var request = new MovieSearchRequest { Title = "Shawshank" };
      var expectedResponse = new MovieSearchResponse
      {
        Search = new List<MovieData>
                {
                    new MovieData { Title = "The Shawshank Redemption", Year = "1994", imdbID = "tt0111161" }
                },
        TotalResults = 1,
        Response = true
      };

      _mockMovieService.Setup(service => service.SearchMoviesAsync(request))
          .ReturnsAsync(expectedResponse);

      // Act
      var result = await _controller.SearchMovies(request);

      // Assert
      ClassicAssert.IsInstanceOf<OkObjectResult>(result.Result);
      var okResult = (OkObjectResult)result.Result;
      ClassicAssert.IsInstanceOf<MovieSearchResponse>(okResult.Value);
      var returnValue = (MovieSearchResponse)okResult.Value;
      ClassicAssert.AreEqual(expectedResponse.TotalResults, returnValue.TotalResults);
      ClassicAssert.AreEqual(expectedResponse.Search.Count, returnValue.Search.Count);
      ClassicAssert.AreEqual(expectedResponse.Search[0].imdbID, returnValue.Search[0].imdbID);
    }

    [TearDown]
    public void Cleanup()
    {
      _mockMovieService = null;
      _controller = null;
    }
  }
}