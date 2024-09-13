using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using Xunit;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.Lookups
{
    public partial class LookupServiceTests
    {
        [Fact]
        public async Task ShouldModifyLookupAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Lookup randomLookup = CreateRandomModifyLookup(randomDateTimeOffset);
            Lookup inputLookup = randomLookup;
            Lookup storageLookup = inputLookup.DeepClone();
            storageLookup.UpdatedDate = randomLookup.CreatedDate;
            Lookup updatedLookup = inputLookup;
            Lookup expectedLookup = updatedLookup.DeepClone();
            Guid lookupId = inputLookup.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateLookupAsync(inputLookup))
                    .ReturnsAsync(updatedLookup);

            // when
            Lookup actualLookup =
                await this.lookupService.ModifyLookupAsync(inputLookup);

            // then
            actualLookup.Should().BeEquivalentTo(expectedLookup);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLookupAsync(inputLookup),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}