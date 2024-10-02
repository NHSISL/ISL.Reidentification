// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses.Exceptions;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationService
    {
        private static void ValidateAccessRequestIsNotNull(AccessRequest accessRequest)
        {
            if (accessRequest is null)
            {
                throw new NullAccessRequestException("Access request is null.");
            }
        }

        private async ValueTask ValidateUserEmail(string userEmail)
        {
            Validate(
                (Rule: await IsInvalidAsync(userEmail), Parameter: nameof(userEmail)));
        }

        private async ValueTask ValidateIdentifierAndOrgs(string identifier, List<string> orgs)
        {
            Validate(
                (Rule: await IsInvalidAsync(identifier), Parameter: nameof(identifier)),
                (Rule: await IsInvalidAsync(orgs), Parameter: nameof(orgs)));
        }

        private static async ValueTask<dynamic> IsInvalidAsync(string name) => new
        {
            Condition = String.IsNullOrWhiteSpace(name),
            Message = "Text is invalid"
        };

        private static async ValueTask<dynamic> IsInvalidAsync(List<string> strings) => new
        {
            Condition = strings is null || strings.Where(stringText => String.IsNullOrWhiteSpace(stringText)).Any(),
            Message = "List of text is invalid"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentAccessOrchestrationException =
                new InvalidArgumentAccessOrchestrationException(
                    message: "Invalid argument access orchestration exception, " +
                        "please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentAccessOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentAccessOrchestrationException.ThrowIfContainsErrors();
        }
    }
}
