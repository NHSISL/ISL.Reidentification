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
        public async Task ShouldAddLookupAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            Lookup randomLookup = CreateRandomLookup(randomDateTimeOffset);
            Lookup inputLookup = randomLookup;
            Lookup storageLookup = inputLookup;
            Lookup expectedLookup = storageLookup.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLookupAsync(inputLookup))
                    .ReturnsAsync(storageLookup);

            // when
            Lookup actualLookup = await this.lookupService
                .AddLookupAsync(inputLookup);

            // then
            actualLookup.Should().BeEquivalentTo(expectedLookup);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLookupAsync(inputLookup),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}