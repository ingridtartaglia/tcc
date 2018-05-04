using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using TrainingSystem.Models;

namespace TrainingSystemTests.Utils
{
    public class FakeUserManager : UserManager<AppUser>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<AppUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<AppUser>>().Object,
                  new IUserValidator<AppUser>[0],
                  new IPasswordValidator<AppUser>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<AppUser>>>().Object) 
        { }
    }
}
