// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

<<<<<<< HEAD:ISL.ReIdentification.Core/Models/Foundations/AccessAudit/Exceptions/NullAccessAuditException.cs
namespace ISL.ReIdentification.Core.Models.Foundations.AccessAudits.Exceptions
{
    public class NullAccessAuditException : Xeption
    {
        public NullAccessAuditException(string message)
=======
namespace ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions
{
    public class NullImpersonationContextException : Xeption
    {
        public NullImpersonationContextException(string message)
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core/Models/Foundations/ImpersonationContexts/Exceptions/NullImpersonationContextException.cs
            : base(message)
        { }
    }
}
