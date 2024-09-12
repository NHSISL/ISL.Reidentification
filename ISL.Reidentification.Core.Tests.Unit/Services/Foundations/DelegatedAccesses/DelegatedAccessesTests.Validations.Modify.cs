// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
using Moq;

namespace ISL.Reidentification.Core.Tests.Unit.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessesTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDelegatedAccessIsNullAndLogItAsync()
        {
            //given
            DelegatedAccess nullDelegatedAccess = null;

            var nullDelegatedAccessException =
                new NullDelegatedAccessException(message: "DelegatedAccess is null");

            var expectedDelegatedAccessValidationException =
                new DelegatedAccessValidationException(
                    message: "DelegatedAccess validation error occurred, please fix errors and try again.",
                    innerException: nullDelegatedAccessException);

            // when
            ValueTask<DelegatedAccess> modifyDelegatedAccessTask =
                this.delegatedAccessService.ModifyDelegatedAccessAsync(nullDelegatedAccess);

            DelegatedAccessValidationException actualDelegatedAccessValidationException =
                await Assert.ThrowsAsync<DelegatedAccessValidationException>(
                    modifyDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessValidationException.Should().BeEquivalentTo(
                expectedDelegatedAccessValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(
                    SameExceptionAs(expectedDelegatedAccessValidationException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.UpdateDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
