// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldRemoveImpersonationContextByIdAsync()
        {
            // given
            Guid someImpersonationContextId = Guid.NewGuid();
            ImpersonationContext randomImpersonationContext = CreateRandomImpersonationContext();
            ImpersonationContext storageImpersonationContext = randomImpersonationContext;
            ImpersonationContext inputImpersonationContext = storageImpersonationContext;
            ImpersonationContext removedImpersonationContext = inputImpersonationContext;
            ImpersonationContext expectedImpersonationContext = removedImpersonationContext.DeepClone();

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectImpersonationContextByIdAsync(someImpersonationContextId))
                    .ReturnsAsync(storageImpersonationContext);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.DeleteImpersonationContextAsync(inputImpersonationContext))
                    .ReturnsAsync(removedImpersonationContext);

            // when
            ImpersonationContext actualImpersonationContext =
                await this.impersonationContextService.RemoveImpersonationContextByIdAsync(someImpersonationContextId);

            // then
            actualImpersonationContext.Should().BeEquivalentTo(expectedImpersonationContext);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectImpersonationContextByIdAsync(someImpersonationContextId),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.DeleteImpersonationContextAsync(storageImpersonationContext),
                    Times.Once);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
