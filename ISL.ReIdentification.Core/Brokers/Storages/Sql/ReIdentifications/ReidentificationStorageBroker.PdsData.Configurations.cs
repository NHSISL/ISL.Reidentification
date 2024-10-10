// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications
{
    public partial class ReIdentificationStorageBroker
    {
        private void AddPdsDataConfigurations(EntityTypeBuilder<PdsData> builder)
        {
            builder.HasKey(pdsData => pdsData.RowId);

            //builder.HasMany(pdsData => pdsData.OdsDatas)
            //    .WithOne(odsData => odsData.PdsData)
            //    .HasForeignKey(odsData => odsData.OrganisationCode_Root)
            //    .HasPrincipalKey(pdsData => pdsData.CcgOfRegistration);

            //builder.HasMany(pdsData => pdsData.OdsDatas)
            //    .WithOne(odsData => odsData.PdsData)
            //    .HasForeignKey(odsData => odsData.OrganisationCode_Root)
            //    .HasPrincipalKey(pdsData => pdsData.CurrentCcgOfRegistration);

            //builder.HasMany(pdsData => pdsData.OdsDatas)
            //    .WithOne(odsData => odsData.PdsData)
            //    .HasForeignKey(odsData => odsData.OrganisationCode_Root)
            //    .HasPrincipalKey(pdsData => pdsData.IcbOfRegistration);

            //builder.HasMany(pdsData => pdsData.OdsDatas)
            //    .WithOne(odsData => odsData.PdsData)
            //    .HasForeignKey(odsData => odsData.OrganisationCode_Root)
            //    .HasPrincipalKey(pdsData => pdsData.CurrentIcbOfRegistration);

            builder.Property(pdsData => pdsData.RowId)
                .IsRequired();

            builder.Property(pdsData => pdsData.PseudoNhsNumber)
                .HasMaxLength(15);

            builder.Property(pdsData => pdsData.PrimaryCareProvider)
                .HasMaxLength(8);

            builder.Property(pdsData => pdsData.CcgOfRegistration)
                .HasMaxLength(5);

            builder
                .Property(pdsData => pdsData.CurrentCcgOfRegistration)
                .HasMaxLength(5);

            builder
                .Property(pdsData => pdsData.IcbOfRegistration)
                .HasMaxLength(5);

            builder
                .Property(pdsData => pdsData.CurrentIcbOfRegistration)
                .HasMaxLength(5);
        }
    }
}
