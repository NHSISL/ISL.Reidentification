// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
                (Rule: await IsInvalidEmptyAsync(userEmail), Parameter: nameof(userEmail)));
        }

        private async ValueTask ValidateIdentifierAndOrgs(string identifier, List<string> orgs)
        {
            await ValidateIdentifierAndOrgsNotNull(identifier, orgs);
            await ValidateIdentifierAndOrgsEmpty(identifier, orgs);
        }

        private async ValueTask ValidateIdentifierAndOrgsNotNull(string identifier, List<string> orgs)
        {
            Validate(
                (Rule: await IsInvalidNullAsync(identifier), Parameter: nameof(identifier)),
                (Rule: await IsInvalidNullAsync(orgs), Parameter: nameof(orgs)));
        }

        private async ValueTask ValidateIdentifierAndOrgsEmpty(string identifier, List<string> orgs)
        {
            Validate(
                (Rule: await IsInvalidEmptyAsync(identifier), Parameter: nameof(identifier)),
                (Rule: await IsInvalidEmptyAsync(orgs), Parameter: nameof(orgs)));
        }

        private static async ValueTask<dynamic> IsInvalidNullAsync(string name) => new
        {
            Condition = name is null,
            Message = "Text is invalid"
        };

        private static async ValueTask<dynamic> IsInvalidNullAsync(List<string> strings) => new
        {
            Condition = strings is null,
            Message = "List is invalid"
        };

        private static async ValueTask<dynamic> IsInvalidEmptyAsync(string name) => new
        {
            Condition = String.IsNullOrWhiteSpace(name),
            Message = "Text is invalid"
        };

        private static async ValueTask<dynamic> IsInvalidEmptyAsync(List<string> strings) => new
        {
            Condition = strings.Exists(x => String.IsNullOrWhiteSpace(x)),
            Message = "List is invalid"
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
