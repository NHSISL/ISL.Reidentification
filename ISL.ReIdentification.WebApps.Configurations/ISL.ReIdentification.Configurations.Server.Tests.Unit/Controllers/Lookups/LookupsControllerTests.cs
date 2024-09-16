// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using ISL.ReIdentification.Configurations.Server.Controllers;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Services.Foundations.Lookups;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.Lookups
{
    public partial class LookupsControllerTests
    {

        private readonly Mock<ILookupService> mockLookupService;
        private readonly LookupsController lookupsController;

        public LookupsControllerTests()
        {
            mockLookupService = new Mock<ILookupService>();
            lookupsController = new LookupsController(mockLookupService.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Lookup CreateRandomLookup() =>
            CreateLookupFiller().Create();

        private static IQueryable<Lookup> CreateRandomLookups()
        {
            return CreateLookupFiller()
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Filler<Lookup> CreateLookupFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            var filler = new Filler<Lookup>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(lookup => lookup.CreatedBy).Use(user)
                .OnProperty(lookup => lookup.UpdatedBy).Use(user);

            return filler;
        }
    }
}