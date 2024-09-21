// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using ISL.ReIdentification.Configurations.Server.Controllers;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using ISL.ReIdentification.Core.Services.Foundations.UserAccesses;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.UserAccesses
{
    public partial class UserAccessesControllerTests
    {
        private readonly Mock<IUserAccessService> userAccessServiceMock;
        private readonly UserAccessesController userAccessesController;

        public UserAccessesControllerTests()
        {
            this.userAccessServiceMock = new Mock<IUserAccessService>();
            this.userAccessesController = new UserAccessesController(userAccessServiceMock.Object);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static UserAccess CreateRandomUserAccess() =>
            CreateUserAccessesFiller().Create();

        private static IQueryable<UserAccess> CreateRandomUserAccesses()
        {
            return CreateUserAccessesFiller()
                .Create(GetRandomNumber())
                .AsQueryable();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(2, 10).GetValue();
        private static Filler<UserAccess> CreateUserAccessesFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            var filler = new Filler<UserAccess>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(userAccess => userAccess.CreatedBy).Use(user)
                .OnProperty(userAccess => userAccess.UpdatedBy).Use(user);

            return filler;
        }
    }
}
