// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Reidentification.Core.Models.Foundations.AccessAudit;
using Microsoft.EntityFrameworkCore;

namespace ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications
{
    public partial interface IReidentificationStorageBroker
    {
        public DbSet<AccessAudit> AccessAudit { get; set; }
    }
}
