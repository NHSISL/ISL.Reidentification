// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using ISL.ReIdentification.Core.Services.Orchestrations.Accesses;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationServiceTests
    {
        private readonly Mock<IPatientOrgReferenceStorageBroker> patientOrgReferenceStorageBrokerMock;
        private readonly Mock<IReIdentificationStorageBroker> reIdentificationStorageBrokerMock;
        private readonly AccessOrchestrationService accessOrchestrationService;

        public AccessOrchestrationServiceTests()
        {
            this.patientOrgReferenceStorageBrokerMock = new Mock<IPatientOrgReferenceStorageBroker>();
            this.reIdentificationStorageBrokerMock = new Mock<IReIdentificationStorageBroker>();
            this.accessOrchestrationService =
                new AccessOrchestrationService(patientOrgReferenceStorageBrokerMock.Object,
                    reIdentificationStorageBrokerMock.Object);
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

        private static UserAccess CreateRandomUserAccess() =>
            CreateRandomUserAccess(dateTimeOffset: GetRandomDateTimeOffset());

        private static UserAccess CreateRandomUserAccess(DateTimeOffset dateTimeOffset) =>
            CreateUserAccessesFiller(dateTimeOffset).Create();

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private static string GetRandomStringWithLength(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static Filler<OdsData> CreateOdsDataFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<OdsData>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(odsData => odsData.PdsData).IgnoreIt();

            return filler;
        }

        private static Filler<UserAccess> CreateUserAccessesFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
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
