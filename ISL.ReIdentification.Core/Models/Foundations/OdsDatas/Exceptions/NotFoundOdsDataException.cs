// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.OdsDatas.Exceptions
{
    public class NotFoundOdsDataException : Xeption
    {
        public NotFoundOdsDataException(Guid odsDataId)
            : base(message: $"Couldn't find ODS data with odsDataId: {odsDataId}.")
        { }
    }
}
