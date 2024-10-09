// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldStoreDelegatedAccessRequestAndSendApprovalEmailWhenProcessingNewDelegatedAccessRequest()
        {
            // given
            AccessRequest randomAccessRequest = CreateRandomAccessRequest();
            AccessRequest inputAccessRequest = randomAccessRequest.DeepClone();
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            string recipientEmail = inputAccessRequest.DelegatedAccessRequest.RecipientEmail;
            byte[] delegatedAccessRequestData = inputAccessRequest.DelegatedAccessRequest.Data;
            Stream dataStream = new MemoryStream(delegatedAccessRequestData);
            string dataHash = GetRandomStringWithLength(64);
            AccessRequest expectedAccessRequest = inputAccessRequest.DeepClone();
            expectedAccessRequest.DelegatedAccessRequest.CreatedDate = DateTimeOffset.UtcNow;
            expectedAccessRequest.DelegatedAccessRequest.IsApproved = null;

            IQueryable<DelegatedAccess> delegatedAccesses =
                new List<DelegatedAccess> { randomDelegatedAccess }.AsQueryable();

            this.delegatedAccessServiceMock.Setup(service =>
                service.RetrieveAllDelegatedAccessesAsync())
                .ReturnsAsync(delegatedAccesses);

            this.delegatedAccessServiceMock.Setup(service =>
                service.AddDelegatedAccessAsync(inputAccessRequest.DelegatedAccessRequest))
                .ReturnsAsync(expectedAccessRequest.DelegatedAccessRequest);

            this.hashBrokerMock.Setup(broker =>
                broker.GenerateSha256Hash(dataStream))
                .Returns(dataHash);

            // Setup notification broker

            // when
            AccessRequest actualAccessRequest =
                await this.accessOrchestrationService.ProcessDelegatedAccessRequestAsync(inputAccessRequest);

            // then
            actualAccessRequest.Should().BeEquivalentTo(expectedAccessRequest);

            this.delegatedAccessServiceMock.Verify(service =>
                service.RetrieveAllDelegatedAccessesAsync(),
                Times.Once);

            this.hashBrokerMock.Verify(broker =>
                broker.GenerateSha256Hash(dataStream),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.delegatedAccessServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }
    }
}
