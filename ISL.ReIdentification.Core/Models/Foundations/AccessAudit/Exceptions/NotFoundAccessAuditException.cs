﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

<<<<<<< HEAD:ISL.ReIdentification.Core/Models/Foundations/AccessAudit/Exceptions/NotFoundAccessAuditException.cs
namespace ISL.Reidentification.Core.Models.Foundations.AccessAudits.Exceptions
{
    public class NotFoundAccessAuditException : Xeption
    {
        public NotFoundAccessAuditException(string message)
=======
namespace ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions
{
    public class InvalidImpersonationContextException : Xeption
    {
        public InvalidImpersonationContextException(string message)
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core/Models/Foundations/ImpersonationContexts/Exceptions/InvalidImpersonationContextException.cs
            : base(message)
        { }
    }
}
