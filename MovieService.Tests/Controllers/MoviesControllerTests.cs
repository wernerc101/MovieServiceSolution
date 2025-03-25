using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieService.Api.Controllers;
using MovieService.Api.DTO;
using MovieService.Api.Interfaces;
using NUnit.Framework;
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
      _mockMovieService.Setup(service => service.GetMovieDetailAsync(movieId))
          .ReturnsAsync(new MovieDetail { ImdbID = movieId, Title = "The Shawshank Redemption", Year = "1994" });

      // Act
      var result = await _controller.GetMovie(movieId);

      // Assert
      Assert.IsInstanceOf<OkObjectResult>(result.Result);
      var okResult = (OkObjectResult)result.Result;
      Assert.IsInstanceOf<MovieDetail>(okResult.Value);
      var returnValue = (MovieDetail)okResult.Value;
      Assert.AreEqual(movieId, returnValue.ImdbID);
      Assert.AreEqual("The Shawshank Redemption", returnValue.Title);
    }

    [Test]
    public async Task GetMovie_WithNonExistingId_ReturnsNotFound()
    {
      // Arrange
      string movieId = "nonExistingId";
      _mockMovieService.Setup(service => service.GetMovieDetailAsync(movieId))
          .ReturnsAsync((MovieDetail)null);

      // Act
      var result = await _controller.GetMovie(movieId);

      // Assert
      Assert.IsInstanceOf<NotFoundResult>(result.Result);
    }

    [Test]
    public async Task SearchMovies_WithValidRequest_ReturnsOkResult()
    {
      // Arrange
      var request = new MovieSearchRequest { Title = "Shawshank" };
      var expectedResponse = new MovieSearchResponse
      {
        Search = new System.Collections.Generic.List<MovieSearchResult>
                {
                    new MovieSearchResult { Title = "The Shawshank Redemption", Year = "1994", ImdbID = "tt0111161" }
                },
        TotalResults = "1",
        Response = "True"
      };

      _mockMovieService.Setup(service => service.SearchMoviesAsync(request))
          .ReturnsAsync(expectedResponse);

      // Act
      var result = await _controller.SearchMovies(request);

      // Assert
      Assert.IsInstanceOf<OkObjectResult>(result.Result);
      var okResult = (OkObjectResult)result.Result;
      Assert.IsInstanceOf<MovieSearchResponse>(okResult.Value);
      var returnValue = (MovieSearchResponse)okResult.Value;
      Assert.AreEqual(expectedResponse.TotalResults, returnValue.TotalResults);
      Assert.AreEqual(expectedResponse.Search.Count, returnValue.Search.Count);
      Assert.AreEqual(expectedResponse.Search[0].ImdbID, returnValue.Search[0].ImdbID);
    }

    [TearDown]
    public void Cleanup()
    {
      _mockMovieService = null;
      _controller = null;
    }
  }
}