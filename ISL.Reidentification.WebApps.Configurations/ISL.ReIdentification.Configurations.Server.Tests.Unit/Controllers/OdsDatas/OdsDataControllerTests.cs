// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using ISL.ReIdentification.Configurations.Server.Controllers;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas.Exceptions;
using ISL.ReIdentification.Core.Services.Foundations.OdsDatas;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.OdsDatas
{
    public partial class OdsDataControllerTests : RESTFulController
    {
        private readonly Mock<IOdsDataService> odsDataServiceMock;
        private readonly OdsDataController odsDataController;

        public OdsDataControllerTests()
        {
            this.odsDataServiceMock = new Mock<IOdsDataService>();
            this.odsDataController = new OdsDataController(this.odsDataServiceMock.Object);
        }

        public static TheoryData<Xeption> ValidationExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new OdsDataValidationException(
                    message: someMessage,
                    innerException: someInnerException),
            };
        }

        public static TheoryData<Xeption> ServerExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new OdsDataDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new OdsDataServiceException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static OdsData CreateRandomOdsData() =>
            CreateRandomOdsData(dateTimeOffset: GetRandomDateTimeOffset());

        private static OdsData CreateRandomOdsData(DateTimeOffset dateTimeOffset) =>
            CreateOdsDataFiller(dateTimeOffset).Create();

        private static IQueryable<OdsData> CreateRandomOdsDatas()
        {
            return CreateOdsDataFiller(GetRandomDateTimeOffset())
                .Create(GetRandomNumber())
                .AsQueryable();
        }

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static Filler<OdsData> CreateOdsDataFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<OdsData>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }
    }
}
