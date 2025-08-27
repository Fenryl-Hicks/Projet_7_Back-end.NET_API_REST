using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;
using Xunit;

namespace P7CreateRestApi.Tests.Services
{
    public class RatingServiceTests
    {
        private readonly Mock<IRatingRepository> _repositoryMock;
        private readonly RatingService _service;

        public RatingServiceTests()
        {
            _repositoryMock = new Mock<IRatingRepository>();
            _service = new RatingService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfRatings()
        {
            // Arrange
            var ratings = new List<Rating>
            {
                new Rating { Id = 1, MoodysRating = "AAA" },
                new Rating { Id = 2, MoodysRating = "BBB" }
            };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(ratings);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRating_WhenExists()
        {
            // Arrange
            var rating = new Rating { Id = 1, MoodysRating = "AAA" };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(rating);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("AAA", result!.MoodysRating);
            _repositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnNewRating()
        {
            // Arrange
            var newRating = new Rating { Id = 1, MoodysRating = "AAA" };
            _repositoryMock.Setup(r => r.AddAsync(newRating)).ReturnsAsync(newRating);

            // Act
            var result = await _service.CreateAsync(newRating);

            // Assert
            Assert.Equal(newRating, result);
            _repositoryMock.Verify(r => r.AddAsync(newRating), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenUpdateSucceeds()
        {
            // Arrange
            var rating = new Rating { Id = 1, MoodysRating = "AAA" };
            _repositoryMock.Setup(r => r.UpdateAsync(1, rating)).ReturnsAsync(true);

            // Act
            var result = await _service.UpdateAsync(1, rating);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(r => r.UpdateAsync(1, rating), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenDeleteSucceeds()
        {
            // Arrange
            _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
