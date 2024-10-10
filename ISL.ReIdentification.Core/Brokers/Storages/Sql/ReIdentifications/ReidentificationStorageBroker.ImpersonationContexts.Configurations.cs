// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications
{
    public partial class ReIdentificationStorageBroker
    {
        private void AddImpersonationContextConfigurations(EntityTypeBuilder<ImpersonationContext> builder)
        {
            builder.Property(csvIdentificationRequest => csvIdentificationRequest.RequesterFirstName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(csvIdentificationRequest => csvIdentificationRequest.RequesterLastName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(csvIdentificationRequest => csvIdentificationRequest.RequesterEmail)
                .HasMaxLength(320)
                .IsRequired();

            builder.Property(csvIdentificationRequest => csvIdentificationRequest.RecipientFirstName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(csvIdentificationRequest => csvIdentificationRequest.RecipientLastName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(csvIdentificationRequest => csvIdentificationRequest.RecipientEmail)
                .HasMaxLength(320)
                .IsRequired();

            builder.Property(csvIdentificationRequest => csvIdentificationRequest.Reason)
                .IsRequired();

            builder.Property(csvIdentificationRequest => csvIdentificationRequest.Purpose)
                .IsRequired();

            builder.Property(csvIdentificationRequest => csvIdentificationRequest.Organisation)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(impersonationContext => impersonationContext.IsApproved)
                .IsRequired();

            builder.Property(impersonationContext => impersonationContext.IdentifierColumn)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(impersonationContext => impersonationContext.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(impersonationContext => impersonationContext.CreatedDate)
                .IsRequired();

            builder.Property(impersonationContext => impersonationContext.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(impersonationContext => impersonationContext.UpdatedDate)
                .IsRequired();
        }
    }
}
