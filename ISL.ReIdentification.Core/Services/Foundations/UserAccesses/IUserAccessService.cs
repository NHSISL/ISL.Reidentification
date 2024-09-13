// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;

namespace ISL.ReIdentification.Core.Services.Foundations.UserAccesses
{
    public interface IUserAccessService
    {
        ValueTask<UserAccess> AddUserAccessAsync(UserAccess userAccess);
        ValueTask<UserAccess> ModifyUserAccessAsync(UserAccess userAccess);
    }
}
