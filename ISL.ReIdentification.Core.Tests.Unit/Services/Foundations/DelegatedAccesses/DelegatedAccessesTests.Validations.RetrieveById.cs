﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessesTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenDelegatedAccessIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidDelegatedAccessId = Guid.Empty;

            var invalidDelegatedAccessException =
                new InvalidDelegatedAccessException(
                    message: "Invalid delegated access. Please correct the errors and try again.");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.Id),
                values: "Id is invalid");

            var expectedDelegatedAccessValidationException =
            new DelegatedAccessValidationException(
                    message: "DelegatedAccess validation error occurred, please fix errors and try again.",
                    innerException: invalidDelegatedAccessException);

            // when
            ValueTask<DelegatedAccess> retrieveDelegatedAccessByIdTask =
                this.delegatedAccessService.RetrieveDelegatedAccessByIdAsync(invalidDelegatedAccessId);

            DelegatedAccessValidationException actualDelegatedAccessValidationException =
                await Assert.ThrowsAsync<DelegatedAccessValidationException>(
                    retrieveDelegatedAccessByIdTask.AsTask);

            // then
            actualDelegatedAccessValidationException.Should().BeEquivalentTo(
                expectedDelegatedAccessValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessValidationException))),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfDelegatedAccessIdNotFoundAndLogitAsync()
        {
            //given
            var someDelegatedAccessId = Guid.NewGuid();
            DelegatedAccess nullDelegatedAccess = null;
            var innerException = new Exception();

            var notFoundDelegatedAccessException =
                new NotFoundDelegatedAccessException(
                    message: $"DelegatedAccess not found with id: {someDelegatedAccessId}");

            var expectedDelegatedAccessValidationException =
                new DelegatedAccessValidationException(
                    message: "DelegatedAccess validation error occurred, please fix errors and try again.",
                    innerException: notFoundDelegatedAccessException);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectDelegatedAccessByIdAsync(someDelegatedAccessId))
                    .ReturnsAsync(nullDelegatedAccess);

            // when
            ValueTask<DelegatedAccess> retrieveDelegatedAccessByIdTask =
                this.delegatedAccessService.RetrieveDelegatedAccessByIdAsync(someDelegatedAccessId);

            DelegatedAccessValidationException actualDelegatedAccessValidationException =
                await Assert.ThrowsAsync<DelegatedAccessValidationException>(
                    retrieveDelegatedAccessByIdTask.AsTask);

            // then
            actualDelegatedAccessValidationException.Should().BeEquivalentTo(
                expectedDelegatedAccessValidationException);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(someDelegatedAccessId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessValidationException))),
                    Times.Once);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
