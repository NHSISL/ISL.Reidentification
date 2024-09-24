// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference
{
    public partial class PatientOrgReferenceStorageBroker
    {
        private void AddPdsDataConfigurations(EntityTypeBuilder<PdsData> builder)
        {
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
