﻿// ---------------------------------------------------------
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
        private readonly IReIdentificationStorageBroker reIdentificationStorageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public UserAccessService(
            IReIdentificationStorageBroker reIdentificationStorageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.reIdentificationStorageBroker = reIdentificationStorageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<UserAccess> AddUserAccessAsync(UserAccess userAccess) =>
            TryCatch(async () =>
            {
                await ValidateUserAccessOnAddAsync(userAccess);

                return await this.reIdentificationStorageBroker.InsertUserAccessAsync(userAccess);
            });

        public ValueTask<IQueryable<UserAccess>> RetrieveAllUserAccessesAsync() =>
            TryCatch(this.reIdentificationStorageBroker.SelectAllUserAccessesAsync);

        public ValueTask<UserAccess> RetrieveUserAccessByIdAsync(Guid userAccessId) =>
            TryCatch(async () =>
            {
                await ValidateUserAccessOnRetrieveById(userAccessId);

                var maybeUserAccess = await this.reIdentificationStorageBroker
                    .SelectUserAccessByIdAsync(userAccessId);

                await ValidateStorageUserAccessAsync(maybeUserAccess, userAccessId);

                return maybeUserAccess;
            });

        public ValueTask<UserAccess> ModifyUserAccessAsync(UserAccess userAccess) =>
            TryCatch(async () =>
            {
                await ValidateUserAccessOnModifyAsync(userAccess);

                var maybeUserAccess = await this.reIdentificationStorageBroker
                    .SelectUserAccessByIdAsync(userAccess.Id);

                await ValidateStorageUserAccessAsync(maybeUserAccess, userAccess.Id);
                await ValidateAgainstStorageUserAccessOnModifyAsync(userAccess, maybeUserAccess);

                return await this.reIdentificationStorageBroker.UpdateUserAccessAsync(userAccess);
            });

        public ValueTask<UserAccess> RemoveUserAccessByIdAsync(Guid userAccessId) =>
            TryCatch(async () =>
            {
                await ValidateUserAccessOnRemoveById(userAccessId);

                var maybeUserAccess = await this.reIdentificationStorageBroker
                    .SelectUserAccessByIdAsync(userAccessId);

                await ValidateStorageUserAccessAsync(maybeUserAccess, userAccessId);

                return await this.reIdentificationStorageBroker.DeleteUserAccessAsync(maybeUserAccess);
            });

        public ValueTask<bool> HasAccessToPseudoIdentifier(string userEmail, string pseudoIdentifier) =>
            throw new NotImplementedException();
    }
}
