using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieService.Api.Controllers;
using MovieService.Api.DTO;
using MovieService.Api.Interfaces;

namespace MovieService.Tests.Controllers
{
  [TestFixture]
  public class MoviesControllerTests
  {
    private Mock<IMovieService>? _mockMovieService;
    private MoviesController? _controller;

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
      Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
      var okResult = (OkObjectResult)result.Result;
      Assert.That(okResult.Value, Is.InstanceOf<MovieDetail>());
      var returnValue = (MovieDetail)okResult.Value;
      Assert.That(returnValue.ImdbID, Is.EqualTo(movieId));
      Assert.That(returnValue.Title, Is.EqualTo("The Shawshank Redemption"));
    }

    [Test]
    public async Task GetMovie_WithNonExistingId_ReturnsNotFound()
    {
      // Arrange
      string movieId = "nonExistingId";
      _mockMovieService.Setup(service => service.GetMovieDetailsAsync(movieId))
          .ReturnsAsync((MovieDetail)null);

      // Act
      ActionResult<MovieDetail> result = await _controller.GetMovie(movieId);

      // Assert
      Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task SearchMovies_WithValidRequest_ReturnsOkResult()
    {
      // Arrange
      var request = new MovieSearchRequest { Title = "Shawshank" };
      var expectedResponse = new MovieSearchResponse
      {
        Search =
                [
                    new() { Title = "The Shawshank Redemption", Year = "1994", imdbID = "tt0111161" }
                ],
        TotalResults = 1,
        Response = true
      };

      _mockMovieService.Setup(service => service.SearchMoviesAsync(request))
          .ReturnsAsync(expectedResponse);

      // Act
      var result = await _controller.SearchMovies(request);

      // Assert
      Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
      var okResult = (OkObjectResult)result.Result;
      Assert.That(okResult.Value, Is.InstanceOf<MovieSearchResponse>());
      var returnValue = (MovieSearchResponse)okResult.Value;
      Assert.Multiple(() =>
      {
        Assert.That(returnValue.TotalResults, Is.EqualTo(expectedResponse.TotalResults));
        Assert.That(returnValue.Search.Count, Is.EqualTo(expectedResponse.Search.Count));
      });
      Assert.That(returnValue.Search[0].imdbID, Is.EqualTo(expectedResponse.Search[0].imdbID));
    }

    [TearDown]
    public void Cleanup()
    {
      _mockMovieService = null;
      _controller = null;
    }
  }
}