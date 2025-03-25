using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using MovieService.Api.DTO;
using MovieService.Api.Services;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MovieService.Tests.Services
{
  [TestFixture]
  public class OmdbApiServiceTests
  {
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private HttpClient _httpClient;
    private string _apiKey = "test_api_key";
    private OmdbApiService _service;

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
        Search = new System.Collections.Generic.List<MovieSearchResult>
                {
                    new MovieSearchResult { Title = "The Shawshank Redemption", Year = "1994", ImdbID = "tt0111161" }
                },
        TotalResults = "1",
        Response = "True"
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
      Assert.NotNull(result);
      Assert.AreEqual(expectedResponse.TotalResults, result.TotalResults);
      Assert.AreEqual(expectedResponse.Search.Count, result.Search.Count);
      Assert.AreEqual(expectedResponse.Search[0].ImdbID, result.Search[0].ImdbID);
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
      Assert.NotNull(result);
      Assert.AreEqual(expectedResponse.ImdbID, result.ImdbID);
      Assert.AreEqual(expectedResponse.Title, result.Title);
      Assert.AreEqual(expectedResponse.Director, result.Director);
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