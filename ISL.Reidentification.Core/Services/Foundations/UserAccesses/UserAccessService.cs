// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.Reidentification.Core.Brokers.DateTimes;
using ISL.Reidentification.Core.Brokers.Loggings;
using ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses;

namespace ISL.Reidentification.Core.Services.Foundations.UserAccesses
{
    public partial class UserAccessService : IUserAccessService
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

        public ValueTask<UserAccess> AddUserAccessAsync(UserAccess userAccess) =>
            TryCatch(async () =>
            {
                await ValidateUserAccessOnAddAsync(userAccess);

                return await this.reidentificationStorageBroker.InsertUserAccessAsync(userAccess);
            });

        public ValueTask<UserAccess> ModifyUserAccessAsync(UserAccess userAccess) =>
            TryCatch(async () =>
            {
                await ValidateUserAccessOnModifyAsync(userAccess);

                var maybeUserAccess = await this.reidentificationStorageBroker.SelectUserAccessByIdAsync(userAccess.Id);

                await ValidateStorageUserAccessAsync(maybeUserAccess, userAccess.Id);

                return await this.reidentificationStorageBroker.UpdateUserAccessAsync(userAccess);
            });
    }
}
