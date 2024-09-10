// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;

namespace ISL.Reidentification.Core.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessService
    {
        private static void ValidateDelegatedAccessOnAdd(DelegatedAccess delegatedAccess)
        {
            ValidateDelegatedAccessIsNotNull(delegatedAccess);
        }

        private static void ValidateDelegatedAccessIsNotNull(DelegatedAccess delegatedAccess)
        {
            if (delegatedAccess is null)
            {
                throw new NullDelegatedAccessException("Delegated access is null.");
            }
        }
    }
}
