// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

<<<<<<< HEAD:ISL.ReIdentification.Core/Models/Foundations/OdsDatas/Exceptions/FailedServiceOdsDataException.cs
namespace ISL.ReIdentification.Core.Models.Foundations.OdsDatas.Exceptions
{
    public class FailedServiceOdsDataException : Xeption
    {
        public FailedServiceOdsDataException(string message, Exception innerException)
=======
namespace ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions
{
    public class ImpersonationContextDependencyException : Xeption
    {
        public ImpersonationContextDependencyException(string message, Xeption innerException)
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core/Models/Foundations/ImpersonationContexts/Exceptions/ImpersonationContextDependencyException.cs
            : base(message, innerException)
        { }
    }
}
