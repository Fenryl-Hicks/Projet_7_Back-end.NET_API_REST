using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories; 
using P7CreateRestApi.Services;     
using Xunit;

namespace P7CreateRestApi.Tests.Services
{
    public class TradeServiceTests
    {
        private readonly Mock<ITradeRepository> _repoMock;
        private readonly TradeService _service;

        public TradeServiceTests()
        {
            _repoMock = new Mock<ITradeRepository>();
            _service = new TradeService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnList()
        {
            // Arrange
            var list = new List<Trade>
            {
                new Trade { Id = 1, Account = "ACC-1" },
                new Trade { Id = 2, Account = "ACC-2" }
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
            var entity = new Trade { Id = 42, Account = "ACC-42" };
            _repoMock.Setup(r => r.GetByIdAsync(42)).ReturnsAsync(entity);

            // Act
            var result = await _service.GetByIdAsync(42);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(42, result!.Id);
            _repoMock.Verify(r => r.GetByIdAsync(42), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedEntity()
        {
            // Arrange
            var toCreate = new Trade { Account = "ACC-NEW" };
            var created = new Trade { Id = 100, Account = "ACC-NEW" };
            _repoMock.Setup(r => r.AddAsync(toCreate)).ReturnsAsync(created);

            // Act
            var result = await _service.CreateAsync(toCreate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.Id);
            _repoMock.Verify(r => r.AddAsync(toCreate), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenRepositoryReturnsTrue()
        {
            // Arrange
            var updated = new Trade { Id = 3, Account = "ACC-3" };
            _repoMock.Setup(r => r.UpdateAsync(3, updated)).ReturnsAsync(true);

            // Act
            var ok = await _service.UpdateAsync(3, updated);

            // Assert
            Assert.True(ok);
            _repoMock.Verify(r => r.UpdateAsync(3, updated), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenRepositoryReturnsTrue()
        {
            // Arrange
            _repoMock.Setup(r => r.DeleteAsync(9)).ReturnsAsync(true);

            // Act
            var ok = await _service.DeleteAsync(9);

            // Assert
            Assert.True(ok);
            _repoMock.Verify(r => r.DeleteAsync(9), Times.Once);
        }
    }
}
