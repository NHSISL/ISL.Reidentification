// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses;

namespace ISL.Reidentification.Core.Services.Foundations.UserAccesses
{
    internal interface IUserAccessService
    {
        ValueTask<UserAccess> AddUserAccessAsync(UserAccess userAccess);
    }
}
