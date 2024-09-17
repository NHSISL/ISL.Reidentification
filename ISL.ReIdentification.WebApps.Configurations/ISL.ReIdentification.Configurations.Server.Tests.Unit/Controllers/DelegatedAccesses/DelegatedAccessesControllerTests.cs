// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using ISL.ReIdentification.Configurations.Server.Controllers;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Services.Foundations.DelegatedAccesses;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.DelegatedAccesses
{
    public partial class DelegatedAccessesControllerTests
    {
        private readonly Mock<IDelegatedAccessService> mockDelegatedAccessService;
        private readonly DelegatedAccessesController delegatedAccessesController;

        public DelegatedAccessesControllerTests()
        {
            mockDelegatedAccessService = new Mock<IDelegatedAccessService>();
            delegatedAccessesController = new DelegatedAccessesController(mockDelegatedAccessService.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DelegatedAccess CreateRandomDelegatedAccess() =>
            CreateDelegatedAccessFiller().Create();

        private static IQueryable<DelegatedAccess> CreateRandomDelegatedAccesses()
        {
            return CreateDelegatedAccessFiller()
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Filler<DelegatedAccess> CreateDelegatedAccessFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            var filler = new Filler<DelegatedAccess>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(delegatedAccess => delegatedAccess.CreatedBy).Use(user)
                .OnProperty(delegatedAccess => delegatedAccess.UpdatedBy).Use(user);

            return filler;
        }
    }
}
