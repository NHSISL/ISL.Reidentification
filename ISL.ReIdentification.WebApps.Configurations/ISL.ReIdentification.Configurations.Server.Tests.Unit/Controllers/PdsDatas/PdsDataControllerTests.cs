// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using ISL.ReIdentification.Configurations.Server.Controllers;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas.Exceptions;
using ISL.ReIdentification.Core.Services.Foundations.PdsDatas;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.PdsDatas
{
    public partial class PdsDataControllerTests : RESTFulController
    {
        private readonly Mock<IPdsDataService> pdsDataServiceMock;
        private readonly PdsDataController pdsDataController;

        public PdsDataControllerTests()
        {
            pdsDataServiceMock = new Mock<IPdsDataService>();
            pdsDataController = new PdsDataController(pdsDataServiceMock.Object);
        }

        public static TheoryData<Xeption> ValidationExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new PdsDataValidationException(
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
                new PdsDataDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new PdsDataServiceException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static PdsData CreateRandomPdsData() =>
            CreatePdsDataFiller().Create();

        private static IQueryable<PdsData> CreateRandomPdsDatas()
        {
            return CreatePdsDataFiller()
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Filler<PdsData> CreatePdsDataFiller()
        {
            var filler = new Filler<PdsData>();
            filler.Setup();

            return filler;
        }
    }
}
