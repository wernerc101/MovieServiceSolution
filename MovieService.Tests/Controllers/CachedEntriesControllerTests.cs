using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using MovieService.Api.Controllers;
using MovieService.Api.Interfaces;
using MovieService.Common.Models;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieService.Tests.Controllers
{
  [TestFixture]
  public class CachedEntriesControllerTests
  {
    private Mock<IRepository<CachedEntry>> _mockRepository;
    private CachedEntriesController _controller;

    [SetUp]
    public void Setup()
    {
      _mockRepository = new Mock<IRepository<CachedEntry>>();
      _controller = new CachedEntriesController(_mockRepository.Object);
    }

    [Test]
    public void Get_ReturnsOkResultWithEntries()
    {
      // Arrange
      var cachedEntries = new List<CachedEntry>
      {
                new CachedEntry { Id = 1, Title = "The Shawshank Redemption", Year = "1994" },
                new CachedEntry { Id = 2, Title = "The Godfather", Year = "1972" }
            };

      _mockRepository.Setup(repo => repo.GetAllAsync())
          .Returns((Task<IEnumerable<CachedEntry>>)cachedEntries.AsQueryable());

      // Act
      var result = _controller.Get();

      // Assert
      ClassicAssert.IsInstanceOf<OkObjectResult>(result);
      var okResult = (OkObjectResult)result;
      ClassicAssert.IsInstanceOf<IQueryable<CachedEntry>>(okResult.Value);
      var returnValue = (IQueryable<CachedEntry>)okResult.Value;
      Assert.Equals(2, returnValue.Count());
    }

    [Test]
    public async Task Get_WithValidId_ReturnsOkResultWithEntry()
    {
      // Arrange
      int entryId = 1;
      var cachedEntry = new CachedEntry { Id = entryId, Title = "The Shawshank Redemption", Year = "1994" };

      _mockRepository.Setup(repo => repo.GetByIdAsync(entryId))
          .ReturnsAsync(cachedEntry);

      // Act
      var result = await _controller.Get(entryId);

      // Assert
      ClassicAssert.IsInstanceOf<OkObjectResult>(result);
      var okResult = (OkObjectResult)result;
      ClassicAssert.IsInstanceOf<CachedEntry>(okResult.Value);
      var returnValue = (CachedEntry)okResult.Value;
      ClassicAssert.AreEqual(entryId, returnValue.Id);
    }

    [Test]
    public async Task Get_WithInvalidId_ReturnsNotFound()
    {
      // Arrange
      int entryId = 999;
      _mockRepository.Setup(repo => repo.GetByIdAsync(entryId))
          .ReturnsAsync((CachedEntry)null);

      // Act
      var result = await _controller.Get(entryId);

      // Assert
      ClassicAssert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task Post_WithValidEntry_ReturnsCreatedAtAction()
    {
      // Arrange
      var entryDto = new CachedEntryDto
      {
        Title = "The Shawshank Redemption",
        Year = "1994",
        ImdbID = "tt0111161"
      };

      var cachedEntry = new CachedEntry
      {
        Id = 1,
        Title = entryDto.Title,
        Year = entryDto.Year
        // Other properties would be mapped here
      };

      _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<CachedEntry>()))
          .Callback<CachedEntry>(entry => entry.Id = 1)
          .Returns(Task.CompletedTask);

      // Act
      var result = await _controller.Post(entryDto);

      // Assert
      ClassicAssert.IsInstanceOf<CreatedAtActionResult>(result);
      var createdAtResult = (CreatedAtActionResult)result;
      ClassicAssert.AreEqual("Get", createdAtResult.ActionName);
      ClassicAssert.IsInstanceOf<CachedEntry>(createdAtResult.Value);
      var returnValue = (CachedEntry)createdAtResult.Value;
      ClassicAssert.AreEqual(1, returnValue.Id);
      ClassicAssert.AreEqual(entryDto.Title, returnValue.Title);
    }

    [Test]
    public async Task Put_WithValidIdAndEntry_ReturnsNoContent()
    {
      // Arrange
      int entryId = 1;
      var entryDto = new CachedEntryDto
      {
        Title = "Updated Title",
        Year = "1994"
      };

      var existingEntry = new CachedEntry { Id = entryId, Title = "Original Title", Year = "1994" };

      _mockRepository.Setup(repo => repo.GetByIdAsync(entryId))
          .ReturnsAsync(existingEntry);
      _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<CachedEntry>()))
          .Returns(Task.CompletedTask);

      // Act
      var result = await _controller.Put(entryId, entryDto);

      // Assert
      ClassicAssert.IsInstanceOf<NoContentResult>(result);
    }

    [Test]
    public async Task Put_WithInvalidId_ReturnsNotFound()
    {
      // Arrange
      int entryId = 999;
      var entryDto = new CachedEntryDto
      {
        Title = "Updated Title",
        Year = "1994"
      };

      _mockRepository.Setup(repo => repo.GetByIdAsync(entryId))
          .ReturnsAsync((CachedEntry)null);

      // Act
      var result = await _controller.Put(entryId, entryDto);

      // Assert
      ClassicAssert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task Delete_WithValidId_ReturnsNoContent()
    {
      // Arrange
      int entryId = 1;
      var existingEntry = new CachedEntry { Id = entryId, Title = "The Shawshank Redemption", Year = "1994" };

      _mockRepository.Setup(repo => repo.GetByIdAsync(entryId))
          .ReturnsAsync(existingEntry);
      _mockRepository.Setup(repo => repo.DeleteAsync(entryId))
          .Returns(Task.CompletedTask);

      // Act
      var result = await _controller.Delete(entryId);

      // Assert
      ClassicAssert.IsInstanceOf<NoContentResult>(result);
    }

    [Test]
    public async Task Delete_WithInvalidId_ReturnsNotFound()
    {
      // Arrange
      int entryId = 999;
      _mockRepository.Setup(repo => repo.GetByIdAsync(entryId))
          .ReturnsAsync((CachedEntry)null);

      // Act
      var result = await _controller.Delete(entryId);

      // Assert
      ClassicAssert.IsInstanceOf<NotFoundResult>(result);
    }

    [TearDown]
    public void Cleanup()
    {
      _mockRepository = null;
      _controller = null;
    }
  }
}