// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

<<<<<<< HEAD:ISL.ReIdentification.Core/Models/Foundations/ReIdentifications/Exceptions/FailedClientReIdentificationException.cs
namespace ISL.ReIdentification.Core.Models.Foundations.ReIdentifications.Exceptions
{
    public class FailedClientReIdentificationException : Xeption
    {
        public FailedClientReIdentificationException(string message, Exception innerException, IDictionary data)
            : base(message, innerException, data)
=======
namespace ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions
{
    public class AlreadyExistsImpersonationContextException : Xeption
    {
        public AlreadyExistsImpersonationContextException(string message, Exception innerException, IDictionary data)
                : base(message, innerException, data)
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core/Models/Foundations/ImpersonationContexts/Exceptions/AlreadyExistsImpersonationContextException.cs
        { }
    }
}
