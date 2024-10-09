// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

<<<<<<< HEAD:ISL.ReIdentification.Core/Models/Coordinations/Identifications/Exceptions/IdentificationCoordinationDependencyException.cs
namespace ISL.ReIdentification.Core.Models.Coordinations.Identifications.Exceptions
{
    public class IdentificationCoordinationDependencyException : Xeption
    {
        public IdentificationCoordinationDependencyException(string message, Xeption innerException)
=======
namespace ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions
{
    public class ImpersonationContextDependencyValidationException : Xeption
    {
        public ImpersonationContextDependencyValidationException(string message, Xeption innerException)
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core/Models/Foundations/ImpersonationContexts/Exceptions/ImpersonationContextDependencyValidationException.cs
            : base(message, innerException)
        { }
    }
}
