// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.PdsDatas
{
    public partial class PdsDataServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidPdsDataId = Guid.Empty;

            var invalidPdsDataException =
                new InvalidPdsDataException(
                    message: "Invalid pds data. Please correct the errors and try again.");

            invalidPdsDataException.AddData(
                key: nameof(PdsData.Id),
                values: "Id is invalid");

            var expectedPdsDataValidationException =
                new PdsDataValidationException(
                    message: "PdsData validation error occurred, please fix errors and try again.",
                    innerException: invalidPdsDataException);

            // when
            ValueTask<PdsData> retrievePdsDataByIdTask =
                this.pdsDataService.RetrievePdsDataByIdAsync(invalidPdsDataId);

            PdsDataValidationException actualPdsDataValidationException =
                await Assert.ThrowsAsync<PdsDataValidationException>(
                    retrievePdsDataByIdTask.AsTask);

            // then
            actualPdsDataValidationException.Should()
                .BeEquivalentTo(expectedPdsDataValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsDataValidationException))),
                        Times.Once);

            this.odsStorageBroker.Verify(broker =>
                broker.SelectPdsDataByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.odsStorageBroker.VerifyNoOtherCalls();
        }

    }
}
