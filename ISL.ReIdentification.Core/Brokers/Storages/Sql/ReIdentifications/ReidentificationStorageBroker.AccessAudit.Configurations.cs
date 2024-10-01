﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.ReIdentification.Core.Models.Foundations.AccessAudits;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications
{
    public partial class ReIdentificationStorageBroker
    {
        private void AddAccessAuditConfigurations(EntityTypeBuilder<AccessAudit> builder)
        {
            builder.Property(accessAudit => accessAudit.PseudoIdentifier)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(accessAudit => accessAudit.UserEmail)
                .HasMaxLength(320)
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

            builder.HasIndex(accessAudit => accessAudit.PseudoIdentifier);
            builder.HasIndex(accessAudit => accessAudit.UserEmail);
            builder.HasIndex(accessAudit => accessAudit.HasAccess);
            builder.HasIndex(accessAudit => accessAudit.CreatedDate);
        }
    }
}
