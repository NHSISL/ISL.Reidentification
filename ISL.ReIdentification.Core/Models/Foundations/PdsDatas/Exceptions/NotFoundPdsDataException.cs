// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.PdsDatas.Exceptions
{
    public class NotFoundPdsDataException : Xeption
    {
        public NotFoundPdsDataException(Guid pdsDataId)
            : base(message: $"Couldn't find pds data with pdsDataId: {pdsDataId}.")
        { }
    }
}
