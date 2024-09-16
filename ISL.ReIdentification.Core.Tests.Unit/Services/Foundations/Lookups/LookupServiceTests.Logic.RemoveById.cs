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
        public async Task ShouldRemoveLookupByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputLookupId = randomId;
            Lookup randomLookup = CreateRandomLookup();
            Lookup storageLookup = randomLookup;
            Lookup expectedInputLookup = storageLookup;
            Lookup deletedLookup = expectedInputLookup;
            Lookup expectedLookup = deletedLookup.DeepClone();

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectLookupByIdAsync(inputLookupId))
                    .ReturnsAsync(storageLookup);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.DeleteLookupAsync(expectedInputLookup))
                    .ReturnsAsync(deletedLookup);

            // when
            Lookup actualLookup = await this.lookupService
                .RemoveLookupByIdAsync(inputLookupId);

            // then
            actualLookup.Should().BeEquivalentTo(expectedLookup);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectLookupByIdAsync(inputLookupId),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.DeleteLookupAsync(expectedInputLookup),
                    Times.Once);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}