// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace ISL.Reidentification.Core.Brokers.DateTimes
{
    public interface IDateTimeBroker
    {
        ValueTask<DateTimeOffset> GetCurrentDateTimeOffsetAsync();
    }
}