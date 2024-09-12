// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Reidentification.Core.Models.Foundations.AccessAudit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications
{
    public partial class ReidentificationStorageBroker
    {
        private void AddAccessAuditConfigurations(EntityTypeBuilder<AccessAudit> builder)
        {
            builder.Property(accessAudit => accessAudit.PseudoIdentifier)
                .IsRequired();

            builder.Property(accessAudit => accessAudit.UserEmail)
                .IsRequired();

            builder.Property(accessAudit => accessAudit.HasAccess)
                .IsRequired();

            builder.Property(accessAudit => accessAudit.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(accessAudit => accessAudit.CreatedDate)
                .IsRequired();

            builder.Property(accessAudit => accessAudit.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(accessAudit => accessAudit.UpdatedDate)
                .IsRequired();
        }
    }
}
