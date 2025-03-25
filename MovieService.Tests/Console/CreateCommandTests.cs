using Moq;
using MovieService.Common.Models;
using MovieService.Console.Commands;
using MovieService.Console.Interfaces;

namespace MovieService.Tests.Console
{
  [TestFixture]
  public class CreateCommandTests
  {
    private Mock<IApiClient> _mockApiClient;
    private CreateCommand _command;

    [SetUp]
    public void Setup()
    {
      _mockApiClient = new Mock<IApiClient>();
      _command = new CreateCommand(_mockApiClient.Object);
    }

    [Test]
    public async Task ExecuteAsync_WithValidEntry_CallsCreateCachedEntryAsync()
    {
      // Arrange
      var entry = new CachedEntryDto
      {
        Title = "The Shawshank Redemption",
        Year = "1994",
        Genre = "Drama",
        Director = "Frank Darabont"
      };

      var createdEntry = new CachedEntryDto
      {
        Id = 1,
        Title = entry.Title,
        Year = entry.Year,
        Genre = entry.Genre,
        Director = entry.Director
      };

      _mockApiClient.Setup(client => client.CreateCachedEntryAsync(entry))
          .ReturnsAsync(createdEntry);

      // Act
      await _command.ExecuteAsync(entry);

      // Assert
      _mockApiClient.Verify(client => client.CreateCachedEntryAsync(entry), Times.Once);
    }

    [Test]
    public void ExecuteAsync_WithNullEntry_ThrowsArgumentNullException()
    {
      // Act & Assert
      Assert.ThrowsAsync<ArgumentNullException>(() => _command.ExecuteAsync(null));
    }

    [TearDown]
    public void Cleanup()
    {
      _mockApiClient = null;
      _command = null;
    }
  }
}