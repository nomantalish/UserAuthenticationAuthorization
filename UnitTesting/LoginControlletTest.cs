using Microsoft.AspNetCore.Mvc;
using Moq;
using UserCreationAPI;
using UserCreationAPI.DTOs;
using UserCreationAPI.Services.Contracts;

namespace UnitTesting
{
    public class LoginControlletTest
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;
        public LoginControlletTest()
        {
            // Arrange
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WhenTokenIsGenerated()
        {
            // Arrange
            var input = new LoginDTO { Username = "noman", Password = "123" };
            var expectedToken = "mocked-jwt-token";

            _userServiceMock
                .Setup(service => service.Login(input))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _controller.Login(input);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedToken, okResult.Value);
        }

    }
}