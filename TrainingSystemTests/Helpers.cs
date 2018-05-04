using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TrainingSystemTests
{
    public static class Helpers
    {
        public static UserManager<TUser> TestUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
        {
            if(store == null) {
                var mockStore = new Mock<IUserStore<TUser>>();
                mockStore.As<IUserEmailStore<TUser>>().Setup(x => x.FindByEmailAsync("existed", new System.Threading.CancellationToken())).Returns(Task.FromResult((TUser)null));
                store = mockStore.Object;
            }
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;
            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<TUser>>();
            var validator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<TUser>>();
            pwdValidators.Add(new PasswordValidator<TUser>());
            var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);
            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            return userManager;
        }

        public static void SetupUser(Controller controller, string email)
        {
            var mockContext = new Mock<HttpContext>();
            var claim = new Claim(ClaimTypes.NameIdentifier, email);
            var claims = new List<Claim>
            {
                claim
            };

            mockContext.SetupGet(hc => hc.User.Claims).Returns(claims);
            mockContext.Setup(hc => hc.User.FindFirst(ClaimTypes.NameIdentifier)).Returns(claim);
            controller.ControllerContext = new ControllerContext { HttpContext = mockContext.Object };
        }
    }
}
