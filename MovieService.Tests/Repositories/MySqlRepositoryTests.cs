using Microsoft.EntityFrameworkCore;
using Moq;
using MovieService.Api.Repositories;

namespace MovieService.Tests.Repositories
{
    [TestFixture]
    public class MySqlRepositoryTests
    {
        private Mock<ApplicationDbContext> _mockContext;
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

            _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _mockContext.Setup(c => c.CachedEntries).Returns(_mockDbSet.Object);

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
            Assert.That(result.Id, Is.EqualTo(entryId));
            Assert.That(result.Title, Is.EqualTo("The Shawshank Redemption"));
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
            Assert.That(entries.Count, Is.EqualTo(2));
            Assert.That(entries[0].Title, Is.EqualTo("The Shawshank Redemption"));
            Assert.That(entries[1].Title, Is.EqualTo("The Godfather"));
        }

        [Test]
        public async Task AddAsync_AddsCachedEntryToContext()
        {
            // Arrange
            var newEntry = new CachedEntry { Title = "The Dark Knight", Year = "2008" };

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
            var entry = new CachedEntry { Id = 1, Title = "Updated Title", Year = "1994" };

            // Act
            await _repository.UpdateAsync(entry);

            // Assert
            _mockDbSet.Verify(m => m.Update(entry), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_WithValidId_RemovesCachedEntryFromContext()
        {
            // Arrange
            int entryId = 1;
            _mockDbSet.Setup(m => m.FindAsync(entryId)).ReturnsAsync(_cachedEntries.First());

            // Act
            await _repository.DeleteAsync(entryId);

            // Assert
            _mockDbSet.Verify(m => m.Remove(It.IsAny<CachedEntry>()), Times.Once);
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