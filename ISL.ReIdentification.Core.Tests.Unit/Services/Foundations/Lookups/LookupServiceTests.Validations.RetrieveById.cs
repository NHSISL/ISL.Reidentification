// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.Lookups
{
    public partial class LookupServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidLookupId = Guid.Empty;

            var invalidLookupException =
                new InvalidLookupException(
                    message: "Invalid lookup. Please correct the errors and try again.");

            invalidLookupException.AddData(
                key: nameof(Lookup.Id),
                values: "Id is required");

            var expectedLookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: invalidLookupException);

            // when
            ValueTask<Lookup> retrieveLookupByIdTask =
                this.lookupService.RetrieveLookupByIdAsync(invalidLookupId);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    retrieveLookupByIdTask.AsTask);

            // then
            actualLookupValidationException.Should()
                .BeEquivalentTo(expectedLookupValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedLookupValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectLookupByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfLookupIsNotFoundAndLogItAsync()
        {
            //given
            Guid someLookupId = Guid.NewGuid();
            Lookup noLookup = null;

            var notFoundLookupException =
                new NotFoundLookupException(someLookupId);

            var expectedLookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: notFoundLookupException);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectLookupByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noLookup);

            //when
            ValueTask<Lookup> retrieveLookupByIdTask =
                this.lookupService.RetrieveLookupByIdAsync(someLookupId);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    retrieveLookupByIdTask.AsTask);

            //then
            actualLookupValidationException.Should().BeEquivalentTo(expectedLookupValidationException);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectLookupByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedLookupValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}