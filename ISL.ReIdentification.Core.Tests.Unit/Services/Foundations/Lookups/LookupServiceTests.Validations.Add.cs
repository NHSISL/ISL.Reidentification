using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;
using Xunit;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.Lookups
{
    public partial class LookupServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfLookupIsNullAndLogItAsync()
        {
            // given
            Lookup nullLookup = null;

            var nullLookupException =
                new NullLookupException(message: "Lookup is null.");

            var expectedLookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: nullLookupException);

            // when
            ValueTask<Lookup> addLookupTask =
                this.lookupService.AddLookupAsync(nullLookup);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    addLookupTask.AsTask);

            // then
            actualLookupValidationException.Should().BeEquivalentTo(expectedLookupValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}