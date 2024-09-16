// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.AccessAudits;

namespace ISL.ReIdentification.Core.Services.Foundations.AccessAudits
{
    public interface IAccessAuditService
    {
        ValueTask<AccessAudit> AddAccessAuditAsync(AccessAudit accessAudit);
    }
}
