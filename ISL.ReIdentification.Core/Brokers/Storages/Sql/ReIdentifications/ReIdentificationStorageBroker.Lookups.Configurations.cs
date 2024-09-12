// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications
{
    public partial class ReIdentificationStorageBroker
    {
        private void AddLookupConfigurations(EntityTypeBuilder<Lookup> builder)
        {
            builder.Property(userAccess => userAccess.Name)
                .HasMaxLength(450)
                .IsRequired();

            builder.HasIndex(userAccess => userAccess.Name)
                .IsUnique();

            builder.Property(userAccess => userAccess.Value)
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