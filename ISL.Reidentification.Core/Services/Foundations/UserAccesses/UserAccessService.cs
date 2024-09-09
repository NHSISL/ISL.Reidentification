// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using ISL.Reidentification.Core.Brokers.DateTimes;
using ISL.Reidentification.Core.Brokers.Loggings;
using ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses;

namespace ISL.Reidentification.Core.Services.Foundations.UserAccesses
{
    internal class UserAccessService : IUserAccessService
    {
        private readonly IReidentificationStorageBroker reidentificationStorageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public UserAccessService(
            IReidentificationStorageBroker reidentificationStorageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.reidentificationStorageBroker = reidentificationStorageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<UserAccess> AddUserAccessAsync(UserAccess userAccess) => 
            await this.reidentificationStorageBroker.InsertUserAccessAsync(userAccess);
    }
}
