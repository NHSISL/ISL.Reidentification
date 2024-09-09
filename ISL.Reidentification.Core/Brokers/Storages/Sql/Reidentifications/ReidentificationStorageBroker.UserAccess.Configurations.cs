// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using EFxceptions;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications
{
    public partial class ReidentificationStorageBroker
    {
        private void AddUserAccessConfigurations(EntityTypeBuilder<UserAccess> builder)
        {
            builder.Property(userAccess => userAccess.UserEmail)
                .IsRequired();

            builder.Property(userAccess => userAccess.RecipientEmail)
                .IsRequired();

            builder.Property(userAccess => userAccess.OrgCode)
                .IsRequired();

            builder.Property(userAccess => userAccess.ActiveFrom)
                .IsRequired();

            builder.Property(userAccess => userAccess.ActiveTo)
                .IsRequired(false);

            builder
                .Property(audit => audit.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(audit => audit.CreatedDate)
                .IsRequired();

            builder
                .Property(audit => audit.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(audit => audit.UpdatedDate)
                .IsRequired();
        }
    }
    
}