using Moq;
using MovieService.Api.DTO;
using MovieService.Console.Commands;
using MovieService.Console.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieService.Tests.Console
{
  [TestFixture]
  public class SearchCommandTests
  {
    private Mock<IApiClient> _mockApiClient;
    private SearchCommand _command;

    [SetUp]
    public void Setup()
    {
      _mockApiClient = new Mock<IApiClient>();
      _command = new SearchCommand(_mockApiClient.Object);
    }

    [Test]
    public async Task ExecuteAsync_WithTitleAndYear_CallsSearchCachedEntriesAsync()
    {
      // Arrange
      string title = "Shawshank";
      string year = "1994";
      string id = null;

      var expectedResults = new List<CachedEntryDto>
            {
                new CachedEntryDto { Id = 1, Title = "The Shawshank Redemption", Year = "1994" }
            };

      _mockApiClient.Setup(client => client.SearchCachedEntriesAsync(title, year, null))
          .ReturnsAsync(expectedResults);

      // Act
      await _command.ExecuteAsync(title, year, id);

      // Assert
      _mockApiClient.Verify(client => client.SearchCachedEntriesAsync(title, year, null), Times.Once);
    }

    [Test]
    public async Task ExecuteAsync_WithId_CallsSearchCachedEntriesAsync()
    {
      // Arrange
      string title = null;
      string year = null;
      int id = 1;

      var expectedResults = new List<CachedEntryDto>
            {
                new CachedEntryDto { Id = 1, Title = "The Shawshank Redemption", Year = "1994" }
            };

      _mockApiClient.Setup(client => client.SearchCachedEntriesAsync(null, null, id))
          .ReturnsAsync(expectedResults);

      // Act
      await _command.ExecuteAsync(title, year, id.ToString());

      // Assert
      _mockApiClient.Verify(client => client.SearchCachedEntriesAsync(null, null, id), Times.Once);
    }

    [TearDown]
    public void Cleanup()
    {
      _mockApiClient = null;
      _command = null;
    }
  }
}