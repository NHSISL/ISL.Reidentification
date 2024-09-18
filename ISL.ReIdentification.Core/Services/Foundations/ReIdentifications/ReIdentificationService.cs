// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using LHDS.Core.Brokers.NECS;

namespace ISL.ReIdentification.Core.Services.Foundations.ReIdentifications
{
    public partial class ReIdentificationService : IReIdentificationService
    {
        private readonly INECSBroker necsBroker;
        private readonly ILoggingBroker loggingBroker;

        public ReIdentificationService(
            INECSBroker necsBroker,
            ILoggingBroker loggingBroker)
        {
            this.necsBroker = necsBroker;
            this.loggingBroker = loggingBroker;
        }
        public async ValueTask<IdentificationRequest> ProcessReidentificationRequests(
            IdentificationRequest identificationRequests) =>
            throw new NotImplementedException();
    }
}
