﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses.Exceptions;
using ISL.ReIdentification.Core.Models.Orchestrations.Identifications.Exceptions;
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

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static AccessRequest CreateRandomAccessRequest() =>
            CreateAccessRequestFiller().Create();

        private static Filler<AccessRequest> CreateAccessRequestFiller()
        {
            var filler = new Filler<AccessRequest>();

            filler.Setup()
                .OnProperty(request => request.ImpersonationContextRequest).Use(CreateRandomImpersonationContext);

            return filler;
        }

        private static IdentificationRequest CreateRandomIdentificationRequest() =>
            CreateIdentificationRequestFiller().Create();

        private static Filler<IdentificationRequest> CreateIdentificationRequestFiller() =>
            new Filler<IdentificationRequest>();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static ImpersonationContext CreateRandomImpersonationContext() =>
            CreateRandomImpersonationContext(dateTimeOffset: GetRandomDateTimeOffset());

        private static ImpersonationContext CreateRandomImpersonationContext(DateTimeOffset dateTimeOffset) =>
            CreateImpersonationContextsFiller(dateTimeOffset).Create();

        private static Filler<ImpersonationContext> CreateImpersonationContextsFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<ImpersonationContext>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AccessOrchestrationValidationException(
                    message: "Access orchestration validation errors occured, please try again.",
                    innerException),

                new AccessOrchestrationDependencyValidationException(
                    message: "Access orchestration dependency validation occurred, please try again.",
                    innerException),

                new IdentificationOrchestrationValidationException(
                    message: "Identification orchestration validation errors occurred, please try again.",
                    innerException),

                new IdentificationOrchestrationDependencyValidationException(
                    message: "Identification orchestration dependency validation occurred, please try again.",
                    innerException),
            };
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AccessOrchestrationServiceException(
                    message: "Access orchestration service error occurred, please contact support.",
                    innerException),

                new AccessOrchestrationDependencyException(
                    message: "Access orchestration dependency error occurred, please contact support.",
                    innerException),

                new IdentificationOrchestrationServiceException(
                    message: "Identification orchestration service error occurred, please contact support.",
                    innerException),

                new IdentificationOrchestrationDependencyException(
                    message: "Identification orchestration dependency error occurred, please contact support.",
                    innerException),
            };
        }
    }
}
