﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

<<<<<<< HEAD
=======
<<<<<<< HEAD:ISL.ReIdentification.Core/Models/Foundations/AccessAudit/Exceptions/AccessAuditDependencyException.cs
namespace ISL.ReIdentification.Core.Models.Foundations.AccessAudits.Exceptions
{
    public class AccessAuditDependencyException : Xeption
    {
        public AccessAuditDependencyException(string message, Xeption innerException)
=======
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations
namespace ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions
{
    public class ImpersonationContextServiceException : Xeption
    {
        public ImpersonationContextServiceException(string message, Xeption innerException)
<<<<<<< HEAD
=======
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core/Models/Foundations/ImpersonationContexts/Exceptions/ImpersonationContextServiceException.cs
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations
            : base(message, innerException)
        { }
    }
}
