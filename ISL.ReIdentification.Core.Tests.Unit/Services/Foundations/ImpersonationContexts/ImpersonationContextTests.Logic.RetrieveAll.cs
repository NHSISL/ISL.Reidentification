// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.ImpersonationContexts
{
    public partial class ImpersonationContextsTests
    {
        [Fact]
        public async Task ShouldRetrieveAllImpersonationContextsAsync()
        {
            //given
            IQueryable<ImpersonationContext> randomImpersonationContexts = CreateRandomImpersonationContexts();
            IQueryable<ImpersonationContext> storageImpersonationContexts = randomImpersonationContexts;
            IQueryable<ImpersonationContext> expectedImpersonationContexts = storageImpersonationContexts;

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectAllImpersonationContextsAsync())
                    .ReturnsAsync(storageImpersonationContexts);

            //when
            IQueryable<ImpersonationContext> actualImpersonationContexts =
                await this.impersonationContextService.RetrieveAllImpersonationContextsAsync();

            // then
            actualImpersonationContexts.Should().BeEquivalentTo(expectedImpersonationContexts);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectAllImpersonationContextsAsync(),
                    Times.Once);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}