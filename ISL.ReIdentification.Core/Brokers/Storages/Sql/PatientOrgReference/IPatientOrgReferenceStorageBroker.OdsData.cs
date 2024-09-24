// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference
{
    public partial interface IPatientOrgReferenceStorageBroker
    {
        ValueTask<IQueryable<OdsData>> SelectAllOdsDatasAsync();
        ValueTask<OdsData> SelectOdsDataByIdAsync(Guid odsDataId);
    }
}
