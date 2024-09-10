﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;

namespace ISL.Reidentification.Core.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessService
    {
        private async ValueTask ValidateDelegatedAccessOnAdd(DelegatedAccess delegatedAccess)
        {
            ValidateDelegatedAccessIsNotNull(delegatedAccess);
            Validate(
                (Rule: await IsInvalidAsync(delegatedAccess.Id), Parameter: nameof(DelegatedAccess.Id)),
                (Rule: await IsInvalidAsync(delegatedAccess.RequesterEmail), Parameter: nameof(DelegatedAccess.RequesterEmail)),
                (Rule: await IsInvalidAsync(delegatedAccess.RecipientEmail), Parameter: nameof(DelegatedAccess.RecipientEmail)),
                (Rule: await IsInvalidAsync(delegatedAccess.IdentifierColumn), Parameter: nameof(DelegatedAccess.IdentifierColumn)),
                (Rule: await IsInvalidAsync(delegatedAccess.CreatedBy), Parameter: nameof(DelegatedAccess.CreatedBy)),
                (Rule: await IsInvalidAsync(delegatedAccess.UpdatedBy), Parameter: nameof(DelegatedAccess.UpdatedBy)),
                (Rule: await IsInvalidAsync(delegatedAccess.CreatedDate), Parameter: nameof(DelegatedAccess.CreatedDate)),
                (Rule: await IsInvalidAsync(delegatedAccess.UpdatedDate), Parameter: nameof(DelegatedAccess.UpdatedDate)));
        }

        private static void ValidateDelegatedAccessIsNotNull(DelegatedAccess delegatedAccess)
        {
            if (delegatedAccess is null)
            {
                throw new NullDelegatedAccessException("Delegated access is null.");
            }
        }

        private static async ValueTask<dynamic> IsInvalidAsync(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is invalid"
        };

        private static async ValueTask<dynamic> IsInvalidAsync(string name) => new
        {
            Condition = String.IsNullOrWhiteSpace(name),
            Message = "Text is invalid"
        };

        private static async ValueTask<dynamic> IsInvalidAsync(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is invalid"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDelegatedAccessException =
                new InvalidDelegatedAccessException(
                    message: "Invalid delegated access. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDelegatedAccessException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDelegatedAccessException.ThrowIfContainsErrors();
        }
    }
}
