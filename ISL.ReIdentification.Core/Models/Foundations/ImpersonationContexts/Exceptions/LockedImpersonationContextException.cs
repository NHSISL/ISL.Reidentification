// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

<<<<<<< HEAD
=======
<<<<<<< HEAD:ISL.ReIdentification.Core/Models/Foundations/AccessAudit/Exceptions/FailedOperationAccessAuditException.cs
namespace ISL.ReIdentification.Core.Models.Foundations.AccessAudits.Exceptions
{
    public class FailedOperationAccessAuditException : Xeption
    {
        public FailedOperationAccessAuditException(string message, Exception innerException)
=======
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations
namespace ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions
{
    public class LockedImpersonationContextException : Xeption
    {
        public LockedImpersonationContextException(string message, Exception innerException)
<<<<<<< HEAD
=======
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core/Models/Foundations/ImpersonationContexts/Exceptions/LockedImpersonationContextException.cs
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations
            : base(message, innerException)
        { }
    }
}
