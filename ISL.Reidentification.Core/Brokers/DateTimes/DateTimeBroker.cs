// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace GitFyle.Core.Api.Brokers.DateTimes
{
    internal class DateTimeBroker : IDateTimeBroker
    {
        public async ValueTask<DateTimeOffset> GetCurrentDateTimeOffsetAsync() =>
            DateTimeOffset.UtcNow;
    }
}