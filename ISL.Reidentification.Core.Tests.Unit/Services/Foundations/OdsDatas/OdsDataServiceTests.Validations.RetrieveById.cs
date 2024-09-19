// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.OdsDatas
{
    public partial class OdsDataServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveOdsDataByIdWithInvalidIdAndLogItAsync()
        {
            // given
            Guid invalidOdsDataId = Guid.Empty;

            var invalidOdsDataException = new InvalidOdsDataException(
                message: "Invalid ODS data. Please correct the errors and try again.");

            invalidOdsDataException.AddData(
                key: nameof(OdsData.Id),
                values: "Id is invalid.");

            var expectedOdsDataValidationException = new OdsDataValidationException(
                message: "OdsData validation error occurred, please fix errors and try again.",
                innerException: invalidOdsDataException);

            // when
            ValueTask<OdsData> retrieveOdsDataByIdTask =
                this.odsDataService.RetrieveOdsDataByIdAsync(invalidOdsDataId);

            OdsDataValidationException actualOdsDataValidationException =
                await Assert.ThrowsAsync<OdsDataValidationException>(
                    retrieveOdsDataByIdTask.AsTask);

            // then
            actualOdsDataValidationException.Should()
                .BeEquivalentTo(expectedOdsDataValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOdsDataValidationException))),
                        Times.Once);

            this.odsStorageBroker.Verify(broker =>
                broker.SelectOdsDataByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.odsStorageBroker.VerifyNoOtherCalls();
        }
    }
}
