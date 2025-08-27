using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using P7CreateRestApi.ClassDto;        
using P7CreateRestApi.Controllers;     
using P7CreateRestApi.Entities;        
using Xunit;

// Alias pour lever l'ambiguïté CS0104
using IdentitySignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace P7CreateRestApi.Tests.Controllers
{
    public class LoginsControllerTests
    {
        // Helpers pour mocker UserManager/SignInManager proprement 

        private static Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(
                store.Object,
                Options.Create(new IdentityOptions()),
                new PasswordHasher<User>(),
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                null,
                new Mock<ILogger<UserManager<User>>>().Object
            );
        }

        private static Mock<SignInManager<User>> MockSignInManager(UserManager<User> userManager)
        {
            return new Mock<SignInManager<User>>(
                userManager,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                Options.Create(new IdentityOptions()),
                new Mock<ILogger<SignInManager<User>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<User>>().Object
            );
        }

        private static IConfiguration BuildJwtConfig()
        {
            // Configuration en mémoire pour la génération du JWT dans le controller
            var dict = new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "SuperSecretKey_For_Tests_Change_Me",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience"
            };
            return new ConfigurationBuilder().AddInMemoryCollection(dict!).Build();
        }


        [Fact]
        public async Task Login_should_return_Unauthorized_when_user_not_found()
        {
            // Arrange
            var userManager = MockUserManager();
            userManager.Setup(um => um.FindByEmailAsync("nobody@test.com"))
                       .ReturnsAsync((User?)null);

            var signInManager = MockSignInManager(userManager.Object);
            var config = BuildJwtConfig();

            var controller = new LoginsController(userManager.Object, signInManager.Object, config);

            var dto = new LoginDto { Email = "nobody@test.com", Password = "x" };

            // Act
            var result = await controller.Login(dto, CancellationToken.None);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorized.StatusCode);
        }

        [Fact]
        public async Task Login_should_return_Unauthorized_when_bad_password()
        {
            // Arrange
            var user = new User { Id = "u1", Email = "user@test.com", UserName = "user@test.com", Fullname = "User Test" };

            var userManager = MockUserManager();
            userManager.Setup(um => um.FindByEmailAsync(user.Email!))
                       .ReturnsAsync(user);

            var signInManager = MockSignInManager(userManager.Object);
            signInManager.Setup(sm => sm.PasswordSignInAsync(user.UserName!, "wrong", false, true))
                         .ReturnsAsync(IdentitySignInResult.Failed); // alias pour éviter CS0104

            var config = BuildJwtConfig();

            var controller = new LoginsController(userManager.Object, signInManager.Object, config);

            var dto = new LoginDto { Email = user.Email!, Password = "wrong" };

            // Act
            var result = await controller.Login(dto, CancellationToken.None);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorized.StatusCode);
        }

        [Fact]
        public async Task Login_should_return_OK_with_token_on_success()
        {
            // Arrange
            var user = new User { Id = "u1", Email = "user@test.com", UserName = "user@test.com", Fullname = "User Test" };

            var userManager = MockUserManager();
            userManager.Setup(um => um.FindByEmailAsync(user.Email!))
                       .ReturnsAsync(user);

            var signInManager = MockSignInManager(userManager.Object);
            signInManager.Setup(sm => sm.PasswordSignInAsync(user.UserName!, "Pwd#12345", false, true))
                         .ReturnsAsync(IdentitySignInResult.Success);

            var config = BuildJwtConfig();

            var controller = new LoginsController(userManager.Object, signInManager.Object, config);

            var dto = new LoginDto { Email = user.Email!, Password = "Pwd#12345" };

            // Act
            var result = await controller.Login(dto, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = ok.Value!;

            // helper local pour lire une propriété en ignorant la casse
            static object? GetProp(object obj, string name)
            {
                var pi = obj.GetType().GetProperty(
                    name,
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.IgnoreCase
                );
                return pi?.GetValue(obj);
            }

            var token = GetProp(body, "token");
            var expiresAtUtc = GetProp(body, "expiresAtUtc");
            var email = GetProp(body, "email");
            var fullname = GetProp(body, "fullname");

            Assert.NotNull(token);         // peu importe la casse: token/Token
            Assert.NotNull(expiresAtUtc);
            Assert.Equal(user.Email, email as string);
            Assert.Equal(user.Fullname, fullname as string);
        }

    }
}
