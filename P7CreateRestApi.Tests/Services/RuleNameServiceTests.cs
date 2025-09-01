using Moq;
using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;

namespace P7CreateRestApi.Tests.Services
{
    public class RuleNameServiceTests
    {
        private readonly Mock<IRuleNameRepository> _repoMock;
        private readonly RuleNameService _service;

        public RuleNameServiceTests()
        {
            _repoMock = new Mock<IRuleNameRepository>();
            _service = new RuleNameService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnList()
        {
            // Arrange
            var list = new List<RuleName>
            {
                new RuleName { Id = 1, Name = "Rule-1" },
                new RuleName { Id = 2, Name = "Rule-2" }
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenExists()
        {
            // Arrange
            var entity = new RuleName { Id = 10, Name = "R-10" };
            _repoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(entity);

            // Act
            var result = await _service.GetByIdAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result!.Id);
            _repoMock.Verify(r => r.GetByIdAsync(10), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedEntity()
        {
            // Arrange
            var toCreate = new RuleName { Name = "New" };
            var created = new RuleName { Id = 99, Name = "New" };
            _repoMock.Setup(r => r.AddAsync(toCreate)).ReturnsAsync(created);

            // Act
            var result = await _service.CreateAsync(toCreate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(99, result.Id);
            _repoMock.Verify(r => r.AddAsync(toCreate), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenRepositoryReturnsTrue()
        {
            // Arrange
            var updated = new RuleName { Id = 5, Name = "Upd" };
            _repoMock.Setup(r => r.UpdateAsync(5, updated)).ReturnsAsync(true);

            // Act
            var ok = await _service.UpdateAsync(5, updated);

            // Assert
            Assert.True(ok);
            _repoMock.Verify(r => r.UpdateAsync(5, updated), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenRepositoryReturnsTrue()
        {
            // Arrange
            _repoMock.Setup(r => r.DeleteAsync(7)).ReturnsAsync(true);

            // Act
            var ok = await _service.DeleteAsync(7);

            // Assert
            Assert.True(ok);
            _repoMock.Verify(r => r.DeleteAsync(7), Times.Once);
        }
    }
}
