// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.ReIdentification.Core.Models.Orchestrations.Accesses.Exceptions
{
    public class AccessValidationOrchestrationException : Xeption
    {
        public AccessValidationOrchestrationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
