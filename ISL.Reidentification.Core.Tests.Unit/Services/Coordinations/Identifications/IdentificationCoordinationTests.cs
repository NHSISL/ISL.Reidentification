// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Services.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Services.Orchestrations.Identifications;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Coordinations.Identifications
{
    public partial class IdentificationCoordinationTests
    {
        private readonly Mock<IIdentificationOrchestrationService> identificationOrchestrationServiceMock;
        private readonly Mock<IAccessOrchestrationService> accessOrchestrationServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IdentificationCoordinationService identificationCoordinationService;
        private readonly ICompareLogic compareLogic;

        public IdentificationCoordinationTests()
        {
            this.identificationOrchestrationServiceMock = new Mock<IIdentificationOrchestrationService>();
            this.accessOrchestrationServiceMock = new Mock<IAccessOrchestrationService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.identificationCoordinationService = new IdentificationCoordinationService(
                this.accessOrchestrationServiceMock.Object,
                this.identificationOrchestrationServiceMock.Object,
                this.loggingBrokerMock.Object);
        }

        private static AccessRequest CreateRandomAccessRequest() =>
            CreateAccessRequestFiller().Create();

        private static Filler<AccessRequest> CreateAccessRequestFiller()
        {
            var filler = new Filler<AccessRequest>();

            filler.Setup()
                .OnProperty(request => request.DelegatedAccessRequest).Use(CreateRandomDelegatedAccess);

            return filler;
        }

        private static IdentificationRequest CreateRandomIdentificationRequest() =>
            CreateIdentificationRequestFiller().Create();

        private static Filler<IdentificationRequest> CreateIdentificationRequestFiller() =>
            new Filler<IdentificationRequest>();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DelegatedAccess CreateRandomDelegatedAccess() =>
            CreateRandomDelegatedAccess(dateTimeOffset: GetRandomDateTimeOffset());

        private static DelegatedAccess CreateRandomDelegatedAccess(DateTimeOffset dateTimeOffset) =>
            CreateDelegatedAccessesFiller(dateTimeOffset).Create();

        private static Filler<DelegatedAccess> CreateDelegatedAccessesFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<DelegatedAccess>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
    }
}
