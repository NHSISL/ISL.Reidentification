// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses.Exceptions;
using ISL.ReIdentification.Core.Services.Orchestrations.Accesses;
using Moq;
using Xeptions;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(LoopDependencyValidationExceptions))]
        public async Task
            ShouldThrowAggregateDependencyValidationOnValidateAccessForIdentificationRequestIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            List<Exception> exceptions = new List<Exception>();
            var accessOrchestrationServiceMock = new Mock<AccessOrchestrationService>
                (this.dateTimeBrokerMock.Object,
                this.reIdentificationStorageBrokerMock.Object,
                this.patientOrgReferenceStorageBrokerMock.Object,
                this.loggingBrokerMock.Object)
            { CallBase = true };

            AccessOrchestrationService accessOrchestrationService = accessOrchestrationServiceMock.Object;

            AccessRequest someAccessRequest = CreateRandomAccessRequest();
            string userEmail = GetRandomStringWithLength(10);
            string userOrganisation = GetRandomStringWithLength(5);

            List<string> userOrganisations =
                new List<string> { userOrganisation };

            accessOrchestrationServiceMock.Setup(service =>
                 service.GetOrganisationsForUserAsync(userEmail))
                     .ReturnsAsync(userOrganisations);

            foreach (var identificationItem in someAccessRequest.IdentificationRequest.IdentificationItems)
            {
                accessOrchestrationServiceMock.Setup(service =>
                 service.UserHasAccessToPatientAsync(It.IsAny<string>(), It.IsAny<List<string>>()))
                     .ThrowsAsync(dependencyValidationException);

                var accessOrchestrationDependencyValidationException =
                    new AccessOrchestrationDependencyValidationException(
                        message: "Access orchestration dependency validation error occurred, " +
                            "fix the errors and try again.",
                        innerException: dependencyValidationException.InnerException as Xeption);

                accessOrchestrationDependencyValidationException
                    .AddData("IdentificationItemError", identificationItem.RowNumber);

                exceptions.Add(accessOrchestrationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    message: $"Unable to validate access for {exceptions.Count} identification requests.",
                    exceptions);

            var failedAccessOrchestrationServiceException =
                new FailedServiceAccessOrchestrationException(
                    message: "Failed access aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedAccessOrchestrationServiceException =
                new AccessOrchestrationServiceException(
                    message: "Access orchestration service error occurred, please contact support.",
                    innerException: failedAccessOrchestrationServiceException);

            // when
            ValueTask<AccessRequest> validateAccessForIdentificationRequestTask =
                accessOrchestrationService
                    .ValidateAccessForIdentificationRequestsAsync(someAccessRequest);

            AccessOrchestrationServiceException
                actualAccessOrchestrationServiceException =
                await Assert.ThrowsAsync<AccessOrchestrationServiceException>(
                    validateAccessForIdentificationRequestTask.AsTask);

            // then
            actualAccessOrchestrationServiceException
                .Should().BeEquivalentTo(expectedAccessOrchestrationServiceException);

            foreach (var identificationItem in someAccessRequest.IdentificationRequest.IdentificationItems)
            {
                accessOrchestrationServiceMock.Verify(service =>
                    service.UserHasAccessToPatientAsync(It.IsAny<string>(), It.IsAny<List<string>>()),
                        Times.Once());

                var accessOrchestrationDependencyValidationLoggingException =
                    new AccessOrchestrationDependencyValidationException(
                        message: "Access orchestration dependency validation error occurred, " +
                            "fix the errors and try again.",
                        innerException: dependencyValidationException.InnerException as Xeption);

                accessOrchestrationDependencyValidationLoggingException
                    .AddData("IdentificationItemError", identificationItem.RowNumber);

                this.loggingBrokerMock.Verify(broker =>
                    broker.LogErrorAsync(It.Is(SameExceptionAs(
                        accessOrchestrationDependencyValidationLoggingException))),
                            Times.Once);
            }

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                Times.Once);

            this.reIdentificationStorageBrokerMock.Verify(broker =>
                broker.SelectAllUserAccessesAsync(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   actualAccessOrchestrationServiceException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBrokerMock.VerifyNoOtherCalls();
            this.patientOrgReferenceStorageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(LoopDependencyExceptions))]
        public async Task
            ShouldThrowAggregateDependencyOnValidateAccessForIdentificationRequestIfErrorsInLoopAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            List<Exception> exceptions = new List<Exception>();
            var accessOrchestrationServiceMock = new Mock<AccessOrchestrationService>
                (this.dateTimeBrokerMock.Object,
                this.reIdentificationStorageBrokerMock.Object,
                this.patientOrgReferenceStorageBrokerMock.Object,
                this.loggingBrokerMock.Object)
            { CallBase = true };

            AccessOrchestrationService accessOrchestrationService = accessOrchestrationServiceMock.Object;

            AccessRequest someAccessRequest = CreateRandomAccessRequest();
            string userEmail = GetRandomStringWithLength(10);
            string userOrganisation = GetRandomStringWithLength(5);

            List<string> userOrganisations =
                new List<string> { userOrganisation };

            accessOrchestrationServiceMock.Setup(service =>
                 service.GetOrganisationsForUserAsync(userEmail))
                     .ReturnsAsync(userOrganisations);

            foreach (var identificationItem in someAccessRequest.IdentificationRequest.IdentificationItems)
            {
                accessOrchestrationServiceMock.Setup(service =>
                 service.UserHasAccessToPatientAsync(It.IsAny<string>(), It.IsAny<List<string>>()))
                     .ThrowsAsync(dependencyException);

                var accessOrchestrationDependencyException =
                    new AccessOrchestrationDependencyException(
                        message: "Access orchestration dependency error occurred, " +
                            "fix the errors and try again.",
                        innerException: dependencyException.InnerException as Xeption);

                accessOrchestrationDependencyException
                    .AddData("IdentificationItemError", identificationItem.RowNumber);

                exceptions.Add(accessOrchestrationDependencyException);
            }

            var aggregateException =
                new AggregateException(
                    message: $"Unable to validate access for {exceptions.Count} identification requests.",
                    exceptions);

            var failedAccessOrchestrationServiceException =
                new FailedServiceAccessOrchestrationException(
                    message: "Failed access aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedAccessOrchestrationServiceException =
                new AccessOrchestrationServiceException(
                    message: "Access orchestration service error occurred, please contact support.",
                    innerException: failedAccessOrchestrationServiceException);

            // when
            ValueTask<AccessRequest> validateAccessForIdentificationRequestTask =
                accessOrchestrationService
                    .ValidateAccessForIdentificationRequestsAsync(someAccessRequest);

            AccessOrchestrationServiceException
                actualAccessOrchestrationServiceException =
                await Assert.ThrowsAsync<AccessOrchestrationServiceException>(
                    validateAccessForIdentificationRequestTask.AsTask);

            // then
            actualAccessOrchestrationServiceException
                .Should().BeEquivalentTo(expectedAccessOrchestrationServiceException);

            foreach (var identificationItem in someAccessRequest.IdentificationRequest.IdentificationItems)
            {
                accessOrchestrationServiceMock.Verify(service =>
                    service.UserHasAccessToPatientAsync(It.IsAny<string>(), It.IsAny<List<string>>()),
                        Times.Once());

                var accessOrchestrationDependencyLoggingException =
                    new AccessOrchestrationDependencyException(
                        message: "Access orchestration dependency error occurred, " +
                            "fix the errors and try again.",
                        innerException: dependencyException.InnerException as Xeption);

                accessOrchestrationDependencyLoggingException
                    .AddData("IdentificationItemError", identificationItem.RowNumber);

                this.loggingBrokerMock.Verify(broker =>
                    broker.LogErrorAsync(It.Is(SameExceptionAs(
                        accessOrchestrationDependencyLoggingException))),
                            Times.Once);
            }

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                Times.Once);

            this.reIdentificationStorageBrokerMock.Verify(broker =>
                broker.SelectAllUserAccessesAsync(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   actualAccessOrchestrationServiceException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBrokerMock.VerifyNoOtherCalls();
            this.patientOrgReferenceStorageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnAddIfServiceErrorsInLoopOccurredAndLogItAsync()
        {
            // given
            List<Exception> exceptions = new List<Exception>();
            var serviceException = new Exception();
            var accessOrchestrationServiceMock = new Mock<AccessOrchestrationService>
                (this.dateTimeBrokerMock.Object,
                this.reIdentificationStorageBrokerMock.Object,
                this.patientOrgReferenceStorageBrokerMock.Object,
                this.loggingBrokerMock.Object)
            { CallBase = true };

            AccessOrchestrationService accessOrchestrationService = accessOrchestrationServiceMock.Object;

            AccessRequest someAccessRequest = CreateRandomAccessRequest();
            string userEmail = GetRandomStringWithLength(10);
            string userOrganisation = GetRandomStringWithLength(5);

            List<string> userOrganisations =
                new List<string> { userOrganisation };

            accessOrchestrationServiceMock.Setup(service =>
                 service.GetOrganisationsForUserAsync(userEmail))
                     .ReturnsAsync(userOrganisations);

            var innerFailedAccessOrchestrationServiceException =
                new FailedServiceAccessOrchestrationException(
                    message: "Failed access orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var innerAccessOrchestrationServiceException =
                new AccessOrchestrationServiceException(
                    message: "Access orchestration service error occurred, please contact support.",
                    innerException: innerFailedAccessOrchestrationServiceException);

            foreach (var identificationItem in someAccessRequest.IdentificationRequest.IdentificationItems)
            {
                accessOrchestrationServiceMock.Setup(service =>
                 service.UserHasAccessToPatientAsync(It.IsAny<string>(), It.IsAny<List<string>>()))
                     .ThrowsAsync(serviceException);

                innerAccessOrchestrationServiceException
                    .AddData("IdentificationItemError", identificationItem.RowNumber);

                exceptions.Add(innerAccessOrchestrationServiceException);
            }

            var aggregateException =
                new AggregateException(
                    message: $"Unable to validate access for {exceptions.Count} identification requests.",
                    exceptions);

            var failedAccessOrchestrationServiceException =
                new FailedServiceAccessOrchestrationException(
                    message: "Failed access aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedAccessOrchestrationServiceException =
                new AccessOrchestrationServiceException(
                    message: "Access orchestration service error occurred, please contact support.",
                    innerException: failedAccessOrchestrationServiceException);

            // when
            ValueTask<AccessRequest> validateAccessForIdentificationRequestTask =
                accessOrchestrationService
                    .ValidateAccessForIdentificationRequestsAsync(someAccessRequest);

            AccessOrchestrationServiceException
                actualAccessOrchestrationServiceException =
                await Assert.ThrowsAsync<AccessOrchestrationServiceException>(
                    validateAccessForIdentificationRequestTask.AsTask);

            // then
            actualAccessOrchestrationServiceException
                .Should().BeEquivalentTo(expectedAccessOrchestrationServiceException);

            foreach (var identificationItem in someAccessRequest.IdentificationRequest.IdentificationItems)
            {
                accessOrchestrationServiceMock.Verify(service =>
                    service.UserHasAccessToPatientAsync(It.IsAny<string>(), It.IsAny<List<string>>()),
                        Times.Once());

                this.loggingBrokerMock.Verify(broker =>
                    broker.LogErrorAsync(It.Is(SameExceptionAs(
                        innerAccessOrchestrationServiceException))),
                            Times.Once);
            }

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                Times.Once);

            this.reIdentificationStorageBrokerMock.Verify(broker =>
                broker.SelectAllUserAccessesAsync(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   actualAccessOrchestrationServiceException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBrokerMock.VerifyNoOtherCalls();
            this.patientOrgReferenceStorageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnValidateAccessForIdentificationRequestAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var accessOrchestrationServiceMock = new Mock<AccessOrchestrationService>
                (this.dateTimeBrokerMock.Object,
                this.reIdentificationStorageBrokerMock.Object,
                this.patientOrgReferenceStorageBrokerMock.Object,
                this.loggingBrokerMock.Object)
            { CallBase = true };

            AccessRequest someAccessRequest = CreateRandomAccessRequest();

            accessOrchestrationServiceMock.Setup(service =>
                 service.GetOrganisationsForUserAsync(It.IsAny<string>()))
                     .ThrowsAsync(dependencyValidationException);

            var expectedAccessOrchestrationDependencyValidationException =
                new AccessOrchestrationDependencyValidationException(
                    message: "Access orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            AccessOrchestrationService accessOrchestrationService = accessOrchestrationServiceMock.Object;

            // when
            ValueTask<AccessRequest> validateAccessForIdentificationRequestTask =
                accessOrchestrationService
                    .ValidateAccessForIdentificationRequestsAsync(someAccessRequest);

            AccessOrchestrationDependencyValidationException
                actualAccessOrchestrationDependencyValidationException =
                await Assert.ThrowsAsync<AccessOrchestrationDependencyValidationException>(
                    validateAccessForIdentificationRequestTask.AsTask);

            // then
            actualAccessOrchestrationDependencyValidationException
                .Should().BeEquivalentTo(expectedAccessOrchestrationDependencyValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedAccessOrchestrationDependencyValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBrokerMock.VerifyNoOtherCalls();
            this.patientOrgReferenceStorageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnValidateAccessForIdentificationRequestAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var accessOrchestrationServiceMock = new Mock<AccessOrchestrationService>
                (this.dateTimeBrokerMock.Object,
                this.reIdentificationStorageBrokerMock.Object,
                this.patientOrgReferenceStorageBrokerMock.Object,
                this.loggingBrokerMock.Object)
            { CallBase = true };

            AccessRequest someAccessRequest = CreateRandomAccessRequest();

            accessOrchestrationServiceMock.Setup(service =>
                 service.GetOrganisationsForUserAsync(It.IsAny<string>()))
                     .ThrowsAsync(dependencyException);

            var expectedAccessOrchestrationDependencyException =
                new AccessOrchestrationDependencyException(
                    message: "Access orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            AccessOrchestrationService accessOrchestrationService = accessOrchestrationServiceMock.Object;

            // when
            ValueTask<AccessRequest> validateAccessForIdentificationRequestTask =
                accessOrchestrationService
                    .ValidateAccessForIdentificationRequestsAsync(someAccessRequest);

            AccessOrchestrationDependencyException
                actualAccessOrchestrationDependencyException =
                await Assert.ThrowsAsync<AccessOrchestrationDependencyException>(
                    validateAccessForIdentificationRequestTask.AsTask);

            // then
            actualAccessOrchestrationDependencyException
                .Should().BeEquivalentTo(expectedAccessOrchestrationDependencyException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedAccessOrchestrationDependencyException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBrokerMock.VerifyNoOtherCalls();
            this.patientOrgReferenceStorageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccurredAndLogItAsync()
        {
            var accessOrchestrationServiceMock = new Mock<AccessOrchestrationService>
                (this.dateTimeBrokerMock.Object,
                this.reIdentificationStorageBrokerMock.Object,
                this.patientOrgReferenceStorageBrokerMock.Object,
                this.loggingBrokerMock.Object)
            { CallBase = true };

            AccessRequest someAccessRequest = CreateRandomAccessRequest();
            var serviceException = new Exception();

            var failedServiceAccessOrchestrationException =
                new FailedServiceAccessOrchestrationException(
                    message: "Failed access orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAccessOrchestrationServiceException =
                new AccessOrchestrationServiceException(
                    message: "Access orchestration service error occurred, please contact support.",
                    innerException: failedServiceAccessOrchestrationException);

            accessOrchestrationServiceMock.Setup(service =>
                 service.GetOrganisationsForUserAsync(It.IsAny<string>()))
                     .ThrowsAsync(serviceException);

            AccessOrchestrationService accessOrchestrationService = accessOrchestrationServiceMock.Object;

            // when
            ValueTask<AccessRequest> validateAccessForIdentificationRequestTask =
                accessOrchestrationService
                    .ValidateAccessForIdentificationRequestsAsync(someAccessRequest);

            AccessOrchestrationServiceException
                actualAccessOrchestrationServiceException =
                await Assert.ThrowsAsync<AccessOrchestrationServiceException>(
                    validateAccessForIdentificationRequestTask.AsTask);

            // then
            actualAccessOrchestrationServiceException
                .Should().BeEquivalentTo(expectedAccessOrchestrationServiceException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedAccessOrchestrationServiceException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBrokerMock.VerifyNoOtherCalls();
            this.patientOrgReferenceStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
