// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications
{
    public partial class ReIdentificationStorageBroker
    {
        private void AddDelegatedAccessConfigurations(EntityTypeBuilder<DelegatedAccess> builder)
        {
            builder.Property(delegatedAccess => delegatedAccess.RequesterFirstName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(delegatedAccess => delegatedAccess.RequesterLastName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(delegatedAccess => delegatedAccess.RequesterEmail)
                .HasMaxLength(320)
                .IsRequired();

            builder.Property(delegatedAccess => delegatedAccess.RecipientFirstName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(delegatedAccess => delegatedAccess.RecipientFirstName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(delegatedAccess => delegatedAccess.RecipientEmail)
                .HasMaxLength(320)
                .IsRequired();

            builder.Property(delegatedAccess => delegatedAccess.IsSystemAccess)
                .IsRequired(false);

            builder.Property(delegatedAccess => delegatedAccess.IsApproved)
                .IsRequired();

            builder.Property(delegatedAccess => delegatedAccess.Data)
                .IsRequired(false);

            builder.Property(delegatedAccess => delegatedAccess.IdentifierColumn)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(delegatedAccess => delegatedAccess.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(delegatedAccess => delegatedAccess.CreatedDate)
                .IsRequired();

            builder.Property(delegatedAccess => delegatedAccess.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(delegatedAccess => delegatedAccess.UpdatedDate)
                .IsRequired();
        }
    }
}
