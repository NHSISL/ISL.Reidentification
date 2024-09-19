// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Services.Foundations.Ods;
using Microsoft.AspNetCore.Mvc;

namespace ISL.ReIdentification.Configurations.Server.Controllers
{
    public class OdsDataController
    {
        private readonly IOdsService odsService;

        public OdsDataController(IOdsService odsService) =>
            this.odsService = odsService;

        [HttpGet]
        public async ValueTask<ActionResult<IQueryable<OdsData>>> GetAsync() =>
            throw new NotImplementedException();
    }
}
