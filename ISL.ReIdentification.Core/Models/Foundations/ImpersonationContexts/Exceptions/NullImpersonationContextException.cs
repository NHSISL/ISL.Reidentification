// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

<<<<<<< HEAD
=======
<<<<<<< HEAD:ISL.ReIdentification.Core/Models/Foundations/AccessAudit/Exceptions/NullAccessAuditException.cs
namespace ISL.ReIdentification.Core.Models.Foundations.AccessAudits.Exceptions
{
    public class NullAccessAuditException : Xeption
    {
        public NullAccessAuditException(string message)
=======
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations
namespace ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions
{
    public class NullImpersonationContextException : Xeption
    {
        public NullImpersonationContextException(string message)
<<<<<<< HEAD
=======
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core/Models/Foundations/ImpersonationContexts/Exceptions/NullImpersonationContextException.cs
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations
            : base(message)
        { }
    }
}
