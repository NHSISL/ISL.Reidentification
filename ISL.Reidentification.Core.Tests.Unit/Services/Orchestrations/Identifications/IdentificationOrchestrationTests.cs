// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Identifiers;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using ISL.ReIdentification.Core.Services.Foundations.AccessAudits;
using ISL.ReIdentification.Core.Services.Foundations.ReIdentifications;
using ISL.ReIdentification.Core.Services.Orchestrations.Identifications;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Orchestrations.Identifications
{
    public partial class IdentificationOrchestrationTests
    {
        private readonly Mock<IReIdentificationService> reIdentificationService;
        private readonly Mock<IAccessAuditService> accessAuditService;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly IIdentificationOrchestrationService identificationOrchestrationService;

        public IdentificationOrchestrationTests()
        {
            this.reIdentificationService = new Mock<IReIdentificationService>();
            this.accessAuditService = new Mock<IAccessAuditService>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();

            this.identificationOrchestrationService = new IdentificationOrchestrationService(
                this.reIdentificationService.Object,
                this.accessAuditService.Object,
                this.loggingBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.identifierBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IdentificationRequest CreateRandomIdentificationRequest(bool hasAccess)
        {
            IdentificationRequest randomIdentificationRequest = CreateIdentificationRequestFiller().Create();
            List<IdentificationItem> randomIdentificationItem = CreateIdentificationItemFiller(hasAccess).Create(1).ToList();
            randomIdentificationRequest.IdentificationItems = randomIdentificationItem;

            return randomIdentificationRequest;
        }

        private static Filler<IdentificationRequest> CreateIdentificationRequestFiller() =>
            new Filler<IdentificationRequest>();


        private static Filler<IdentificationItem> CreateIdentificationItemFiller(bool hasAccess)
        {
            var filler = new Filler<IdentificationItem>();

            filler.Setup()
                .OnProperty(item => item.HasAccess).Use(hasAccess);

            return filler;
        }
    }
}
