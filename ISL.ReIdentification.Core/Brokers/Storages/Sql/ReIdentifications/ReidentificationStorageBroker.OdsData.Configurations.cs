// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications
{
    public partial class ReIdentificationStorageBroker
    {
        private void AddOdsDataConfigurations(EntityTypeBuilder<OdsData> builder)
        {
            builder.Property(odsData => odsData.OrganisationCode_Root)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(odsData => odsData.OrganisationPrimaryRole_Root)
                .HasMaxLength(5)
                .IsRequired();

            builder.Property(odsData => odsData.OrganisationCode_Parent)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(odsData => odsData.OrganisationPrimaryRole_Parent)
                .HasMaxLength(5)
                .IsRequired();

            builder.Property(odsData => odsData.RelationshipStartDate)
                .IsRequired();

            builder.Property(odsData => odsData.RelationshipEndDate)
                .IsRequired();

            builder.Property(odsData => odsData.Path)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(odsData => odsData.Depth)
                .IsRequired();
        }
    }
}