// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Reidentification.Core.Models.Foundations.UserAccesses;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions;

namespace ISL.Reidentification.Core.Services.Foundations.UserAccesses
{
    public partial class UserAccessService
    {
        private static void ValidateUserAccessOnAdd(UserAccess userAccess)
        {
            ValidateUserAccessIsNotNull(userAccess);
        }

        private static void ValidateUserAccessIsNotNull(UserAccess userAccess)
        {
            if (userAccess is null)
            {
                throw new NullUserAccessException("User access is null.");
            }
        }
    }
}
