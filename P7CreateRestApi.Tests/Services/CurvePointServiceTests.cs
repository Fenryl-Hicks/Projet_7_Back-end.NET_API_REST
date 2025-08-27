using Moq;
using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P7CreateRestApi.Tests.Services
{
    public class CurvePointServiceTests
    {
        private readonly Mock<ICurvePointRepository> _repoMock;
        private readonly CurvePointService _service;

        public CurvePointServiceTests()
        {
            _repoMock = new Mock<ICurvePointRepository>();
            _service = new CurvePointService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnList()
        {
            // Arrange
            var curves = new List<CurvePoint> { new CurvePoint { Id = 1 } };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(curves);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCurvePoint()
        {
            // Arrange
            var curve = new CurvePoint { Id = 5 };
            _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(curve);

            // Act
            var result = await _service.GetByIdAsync(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result!.Id);
            _repoMock.Verify(r => r.GetByIdAsync(5), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnNewCurvePoint()
        {
            // Arrange
            var input = new CurvePoint { Id = 0 };
            var created = new CurvePoint { Id = 2 };
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
            var updated = new CurvePoint { Id = 3 };
            _repoMock.Setup(r => r.UpdateAsync(3, updated)).ReturnsAsync(true);

            // Act
            var result = await _service.UpdateAsync(3, updated);

            // Assert
            Assert.True(result);
            _repoMock.Verify(r => r.UpdateAsync(3, updated), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenDeleteSucceeds()
        {
            // Arrange
            _repoMock.Setup(r => r.DeleteAsync(10)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(10);

            // Assert
            Assert.True(result);
            _repoMock.Verify(r => r.DeleteAsync(10), Times.Once);
        }
    }
}
