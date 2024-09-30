// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Brokers.NECS.Requests;

namespace LHDS.Core.Brokers.NECS
{
    public interface INECSBroker
    {
        ValueTask<NecsReIdentificationResponse> ReIdAsync(NecsReidentificationRequest necsReidentificationRequest);
    }
}
