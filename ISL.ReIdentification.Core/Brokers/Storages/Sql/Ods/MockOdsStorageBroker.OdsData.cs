// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.Ods
{
    public partial class MockOdsStorageBroker
    {
        public async ValueTask<IQueryable<OdsData>> SelectAllOdsDatasAsync() =>
            new List<OdsData> { CreateOdsData() }.AsQueryable();

        public async ValueTask<OdsData> SelectOdsDataByIdAsync(Guid odsDataId) =>
            CreateOdsData();

        private static OdsData CreateOdsData() =>
            new OdsData { Id = Guid.NewGuid() };
    }
}