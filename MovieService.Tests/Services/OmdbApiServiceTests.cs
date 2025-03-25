using Moq;
using Moq.Protected;
using MovieService.Api.DTO;
using MovieService.Api.Services;
using NUnit.Framework.Legacy;
using System.Text.Json;

namespace MovieService.Tests.Services
{
  [TestFixture]
  public class OmdbApiServiceTests
  {
    private Mock<HttpMessageHandler>? _mockHttpMessageHandler;
    private HttpClient? _httpClient;
    private string _apiKey = "test_api_key";
    private OmdbApiService? _service;

    [SetUp]
    public void Setup()
    {
      _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
      _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
      {
        BaseAddress = new Uri("http://www.omdbapi.com/")
      };
      _service = new OmdbApiService(_httpClient, _apiKey);
    }

    [Test]
    public async Task SearchMoviesAsync_WithValidRequest_ReturnsMovieSearchResponse()
    {
      // Arrange
      var request = new MovieSearchRequest { Title = "Shawshank" };
      var expectedResponse = new MovieSearchResponse
      {
        Search =
                [
                    new MovieData { Title = "The Shawshank Redemption", Year = "1994", imdbID = "tt0111161" }
                ],
        TotalResults = 1,
        Response = true
      };

      var jsonResponse = JsonSerializer.Serialize(expectedResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

      _mockHttpMessageHandler.Protected()
          .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
          )
          .ReturnsAsync(new HttpResponseMessage
          {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonResponse)
          });

      // Act
      var result = await _service.SearchMoviesAsync(request);

      // Assert
      ClassicAssert.NotNull(result);
      Assert.That(result.TotalResults, Is.EqualTo(expectedResponse.TotalResults));
      Assert.That(result.Search.Count, Is.EqualTo(expectedResponse.Search.Count));
      Assert.That(result.Search[0].imdbID, Is.EqualTo(expectedResponse.Search[0].imdbID));
    }

    [Test]
    public async Task GetMovieDetailsAsync_WithValidId_ReturnsMovieDetail()
    {
      // Arrange
      string movieId = "tt0111161";
      var expectedResponse = new MovieDetail
      {
        ImdbID = movieId,
        Title = "The Shawshank Redemption",
        Year = "1994",
        Director = "Frank Darabont"
      };

      var jsonResponse = JsonSerializer.Serialize(expectedResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

      _mockHttpMessageHandler.Protected()
          .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
          )
          .ReturnsAsync(new HttpResponseMessage
          {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonResponse)
          });

      // Act
      var result = await _service.GetMovieDetailsAsync(movieId);

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.Multiple(() =>
      {
        Assert.That(result.ImdbID, Is.EqualTo(expectedResponse.ImdbID));
        Assert.That(result.Title, Is.EqualTo(expectedResponse.Title));
        Assert.That(result.Director, Is.EqualTo(expectedResponse.Director));
      });
    }

    [TearDown]
    public void Cleanup()
    {
      _mockHttpMessageHandler = null;
      _httpClient.Dispose();
      _httpClient = null;
      _service = null;
    }
  }
}