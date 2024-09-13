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
        public async Task ShouldRetrieveLookupByIdAsync()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            Lookup inputLookup = randomLookup;
            Lookup storageLookup = randomLookup;
            Lookup expectedLookup = storageLookup.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLookupByIdAsync(inputLookup.Id))
                    .ReturnsAsync(storageLookup);

            // when
            Lookup actualLookup =
                await this.lookupService.RetrieveLookupByIdAsync(inputLookup.Id);

            // then
            actualLookup.Should().BeEquivalentTo(expectedLookup);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(inputLookup.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}