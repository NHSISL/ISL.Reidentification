// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using EFxceptions;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;
using Microsoft.EntityFrameworkCore;

namespace ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications
{
    public partial class ReidentificationStorageBroker : EFxceptionsContext, IReidentificationStorageBroker
    {
       public DbSet<DelegatedAccess> DelegatedAccesses { get; set; }
    }
}