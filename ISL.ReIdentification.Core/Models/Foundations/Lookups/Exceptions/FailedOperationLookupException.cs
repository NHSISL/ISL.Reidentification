// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

<<<<<<< HEAD:ISL.ReIdentification.Core/Models/Foundations/Lookups/Exceptions/FailedOperationLookupException.cs
namespace ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions
{
    public class FailedOperationLookupException : Xeption
    {
        public FailedOperationLookupException(string message, Exception innerException)
=======
namespace ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions
{
    public class FailedStorageImpersonationContextException : Xeption
    {
        public FailedStorageImpersonationContextException(string message, Exception innerException)
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core/Models/Foundations/ImpersonationContexts/Exceptions/FailedStorageImpersonationContextException.cs
            : base(message, innerException)
        { }
    }
}
