using System.Linq;
using FluentAssertions;
using Moq;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using Xunit;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.Lookups
{
    public partial class LookupServiceTests
    {
        [Fact]
        public void ShouldReturnLookups()
        {
            // given
            IQueryable<Lookup> randomLookups = CreateRandomLookups();
            IQueryable<Lookup> storageLookups = randomLookups;
            IQueryable<Lookup> expectedLookups = storageLookups;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllLookups())
                    .Returns(storageLookups);

            // when
            IQueryable<Lookup> actualLookups =
                this.lookupService.RetrieveAllLookups();

            // then
            actualLookups.Should().BeEquivalentTo(expectedLookups);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllLookups(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}