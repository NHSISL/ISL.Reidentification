// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Reidentification.Core.Models.Foundations.UserAccesses;
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
                .Property(userAccess => userAccess.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(userAccess => userAccess.CreatedDate)
                .IsRequired();

            builder
                .Property(userAccess => userAccess.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(userAccess => userAccess.UpdatedDate)
                .IsRequired();
        }
    }

}