// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.NECS
{
    public interface INECSBroker
    {
        ValueTask<List<string>> ReIdAsync(string pseudoNumber);
    }
}
