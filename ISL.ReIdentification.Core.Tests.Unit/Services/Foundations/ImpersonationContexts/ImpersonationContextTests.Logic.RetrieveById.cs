// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.ImpersonationContexts
{
    public partial class ImpersonationContextsTests
    {
        [Fact]
        public async Task ShouldRetrieveImpersonationContextByIdAsync()
        {
            // given
            ImpersonationContext randomImpersonationContext = CreateRandomImpersonationContext();
            ImpersonationContext storageImpersonationContext = randomImpersonationContext;
            ImpersonationContext expectedImpersonationContext = storageImpersonationContext.DeepClone();

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectImpersonationContextByIdAsync(randomImpersonationContext.Id))
                    .ReturnsAsync(storageImpersonationContext);

            // when
            ImpersonationContext actualImpersonationContext =
                await impersonationContextService.RetrieveImpersonationContextByIdAsync(randomImpersonationContext.Id);

            // then
            actualImpersonationContext.Should().BeEquivalentTo(expectedImpersonationContext);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectImpersonationContextByIdAsync(randomImpersonationContext.Id),
                    Times.Once());

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
