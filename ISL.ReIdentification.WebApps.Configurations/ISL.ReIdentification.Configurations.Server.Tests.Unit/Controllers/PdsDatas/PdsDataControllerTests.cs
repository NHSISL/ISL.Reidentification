// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using ISL.ReIdentification.Configurations.Server.Controllers;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Services.Foundations.PdsDatas;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.PdsDatas
{
    public partial class PdsDataControllerTests
    {
        private readonly Mock<IPdsDataService> mockPdsDataService;
        private readonly PdsDataController pdsDataController;

        public PdsDataControllerTests()
        {
            mockPdsDataService = new Mock<IPdsDataService>();
            pdsDataController = new PdsDataController(mockPdsDataService.Object);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

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
