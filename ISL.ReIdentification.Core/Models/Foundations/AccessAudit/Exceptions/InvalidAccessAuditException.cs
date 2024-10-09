// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

<<<<<<< HEAD:ISL.ReIdentification.Core/Models/Foundations/AccessAudit/Exceptions/InvalidAccessAuditException.cs
namespace ISL.ReIdentification.Core.Models.Foundations.AccessAudits.Exceptions
{
    public class InvalidAccessAuditException : Xeption
    {
        public InvalidAccessAuditException(string message)
=======
namespace ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions
{
    public class NotFoundImpersonationContextException : Xeption
    {
        public NotFoundImpersonationContextException(string message)
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core/Models/Foundations/ImpersonationContexts/Exceptions/NotFoundImpersonationContextException.cs
            : base(message)
        { }
    }
}
