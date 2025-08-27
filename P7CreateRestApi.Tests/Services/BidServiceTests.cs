using Moq;
using P7CreateRestApi.Entities;       
using P7CreateRestApi.Repositories;   
using P7CreateRestApi.Services;       
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P7CreateRestApi.Tests.Services
{
    public class BidServiceTests
    {
        private readonly Mock<IBidRepository> _repoMock;
        private readonly BidService _service;

        public BidServiceTests()
        {
            _repoMock = new Mock<IBidRepository>();
            _service = new BidService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnList()
        {
            // Arrange
            var bids = new List<Bid> { new Bid { Id = 1, Account = "A", BidType = "Spot", BidQuantity = 1 } };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(bids);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnBid_WhenFound()
        {
            // Arrange
            var bid = new Bid { Id = 5, Account = "X", BidType = "Forward", BidQuantity = 2.5 };
            _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(bid);

            // Act
            var result = await _service.GetByIdAsync(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result!.Id);
            _repoMock.Verify(r => r.GetByIdAsync(5), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedBid()
        {
            // Arrange
            var input = new Bid { Account = "A", BidType = "Spot", BidQuantity = 3 };
            var created = new Bid { Id = 2, Account = "A", BidType = "Spot", BidQuantity = 3 };
            _repoMock.Setup(r => r.AddAsync(input)).ReturnsAsync(created);

            // Act
            var result = await _service.CreateAsync(input);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            _repoMock.Verify(r => r.AddAsync(input), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenUpdateSucceeds()
        {
            // Arrange
            var updated = new Bid { Id = 3, Account = "B", BidType = "Swap", BidQuantity = 10 };
            _repoMock.Setup(r => r.UpdateAsync(3, updated)).ReturnsAsync(true);

            // Act
            var ok = await _service.UpdateAsync(3, updated);

            // Assert
            Assert.True(ok);
            _repoMock.Verify(r => r.UpdateAsync(3, updated), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenDeleteSucceeds()
        {
            // Arrange
            _repoMock.Setup(r => r.DeleteAsync(10)).ReturnsAsync(true);

            // Act
            var ok = await _service.DeleteAsync(10);

            // Assert
            Assert.True(ok);
            _repoMock.Verify(r => r.DeleteAsync(10), Times.Once);
        }
    }
}
