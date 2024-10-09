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
        public async Task ShouldAddImpersonationContextAsync()
        {
            //given
            ImpersonationContext randomImpersonationContext = CreateRandomImpersonationContext();
            ImpersonationContext inputImpersonationContext = randomImpersonationContext;
            ImpersonationContext storageImpersonationContext = inputImpersonationContext.DeepClone();
            ImpersonationContext expectedImpersonationContext = inputImpersonationContext.DeepClone();

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.InsertImpersonationContextAsync(inputImpersonationContext))
                    .ReturnsAsync(storageImpersonationContext);

            //when
            ImpersonationContext actualImpersonationContext =
                await this.impersonationContextService.AddImpersonationContextAsync(inputImpersonationContext);

            //then
            actualImpersonationContext.Should().BeEquivalentTo(expectedImpersonationContext);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertImpersonationContextAsync(inputImpersonationContext),
                    Times.Once);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
