// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;

namespace ISL.ReIdentification.Core.Services.Foundations.UserAccesses
{
    public partial class UserAccessService : IUserAccessService
    {
        private readonly IReIdentificationStorageBroker ReIdentificationStorageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public UserAccessService(
            IReIdentificationStorageBroker ReIdentificationStorageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.ReIdentificationStorageBroker = ReIdentificationStorageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<UserAccess> AddUserAccessAsync(UserAccess userAccess) =>
            TryCatch(async () =>
            {
                await ValidateUserAccessOnAddAsync(userAccess);

                return await this.ReIdentificationStorageBroker.InsertUserAccessAsync(userAccess);
            });

        public ValueTask<IQueryable<UserAccess>> RetrieveAllUserAccessesAsync() =>
            throw new NotImplementedException();
    }
}
