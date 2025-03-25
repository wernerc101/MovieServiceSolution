using k8s.KubeConfigModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using MovieService.Api.Models;
using MovieService.Api.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieService.Tests.Repositories
{
  [TestFixture]
  public class MySqlRepositoryTests
  {
    private Mock<DbContext> _mockContext;
    private Mock<DbSet<CachedEntry>> _mockDbSet;
    private MySqlRepository _repository;
    private List<CachedEntry> _cachedEntries;

    [SetUp]
    public void Setup()
    {
      _cachedEntries = new List<CachedEntry>
      {
                new CachedEntry { Id = 1, Title = "The Shawshank Redemption", Year = "1994" },
                new CachedEntry { Id = 2, Title = "The Godfather", Year = "1972" }
      };

      _mockDbSet = new Mock<DbSet<CachedEntry>>();
      _mockDbSet.As<IQueryable<CachedEntry>>().Setup(m => m.Provider).Returns(_cachedEntries.AsQueryable().Provider);
      _mockDbSet.As<IQueryable<CachedEntry>>().Setup(m => m.Expression).Returns(_cachedEntries.AsQueryable().Expression);
      _mockDbSet.As<IQueryable<CachedEntry>>().Setup(m => m.ElementType).Returns(_cachedEntries.AsQueryable().ElementType);
      _mockDbSet.As<IQueryable<CachedEntry>>().Setup(m => m.GetEnumerator()).Returns(_cachedEntries.AsQueryable().GetEnumerator());

      _mockContext = new Mock<DbContext>();
      _mockContext.Setup(c => c.Set<CachedEntry>()).Returns(_mockDbSet.Object);

      _repository = new MySqlRepository(_mockContext.Object);
    }

    [Test]
    public async Task GetByIdAsync_WithValidId_ReturnsCachedEntry()
    {
      // Arrange
      int entryId = 1;
      _mockDbSet.Setup(m => m.FindAsync(entryId)).ReturnsAsync(_cachedEntries.First());

      // Act
      var result = await _repository.GetByIdAsync(entryId);

      // Assert
      Assert.NotNull(result);
      Assert.AreEqual(entryId, result.Id);
      Assert.AreEqual("The Shawshank Redemption", result.Title);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllCachedEntries()
    {
      // Arrange
      _mockDbSet.Setup(m => m.ToListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_cachedEntries);

      // Act
      var result = await _repository.GetAllAsync();

      // Assert
      var entries = result.ToList();
      Assert.AreEqual(2, entries.Count);
      Assert.AreEqual("The Shawshank Redemption", entries[0].Title);
      Assert.AreEqual("The Godfather", entries[1].Title);
    }

    [Test]
    public async Task AddAsync_AddsCachedEntryToContext()
    {
      // Arrange
      var newEntry = new CachedEntry { Title = "Pulp Fiction", Year = "1994" };

      // Act
      await _repository.AddAsync(newEntry);

      // Assert
      _mockDbSet.Verify(m => m.AddAsync(newEntry, It.IsAny<CancellationToken>()), Times.Once);
      _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_UpdatesCachedEntryInContext()
    {
      // Arrange
      var updatedEntry = new CachedEntry { Id = 1, Title = "Updated Title", Year = "1994" };

      // Act
      await _repository.UpdateAsync(updatedEntry);

      // Assert
      _mockDbSet.Verify(m => m.Update(updatedEntry), Times.Once);
      _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_WithValidId_RemovesCachedEntryFromContext()
    {
      // Arrange
      int entryId = 1;
      var entry = _cachedEntries.First();
      _mockDbSet.Setup(m => m.FindAsync(entryId)).ReturnsAsync(entry);

      // Act
      await _repository.DeleteAsync(entryId);

      // Assert
      _mockDbSet.Verify(m => m.Remove(entry), Times.Once);
      _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_WithInvalidId_DoesNothing()
    {
      // Arrange
      int entryId = 999;
      _mockDbSet.Setup(m => m.FindAsync(entryId)).ReturnsAsync((CachedEntry)null);

      // Act
      await _repository.DeleteAsync(entryId);

      // Assert
      _mockDbSet.Verify(m => m.Remove(It.IsAny<CachedEntry>()), Times.Never);
      _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [TearDown]
    public void Cleanup()
    {
      _mockContext = null;
      _mockDbSet = null;
      _repository = null;
      _cachedEntries = null;
    }
  }
}