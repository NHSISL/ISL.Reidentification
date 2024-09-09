// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions;

namespace ISL.Reidentification.Core.Services.Foundations.UserAccesses
{
    public partial class UserAccessService
    {
        private async ValueTask ValidateUserAccessOnAddAsync(UserAccess userAccess)
        {
            ValidateUserAccessIsNotNull(userAccess);

            Validate(
                (Rule: await IsInvalidAsync(userAccess.Id), Parameter: nameof(UserAccess.Id)),
                (Rule: await IsInvalidAsync(userAccess.UserEmail), Parameter: nameof(UserAccess.UserEmail)),
                (Rule: await IsInvalidAsync(userAccess.RecipientEmail), Parameter: nameof(UserAccess.RecipientEmail)),
                (Rule: await IsInvalidAsync(userAccess.OrgCode), Parameter: nameof(UserAccess.OrgCode)),
                (Rule: await IsInvalidAsync(userAccess.ActiveFrom), Parameter: nameof(UserAccess.ActiveFrom)),
                (Rule: await IsInvalidAsync(userAccess.CreatedBy), Parameter: nameof(UserAccess.CreatedBy)),
                (Rule: await IsInvalidAsync(userAccess.UpdatedBy), Parameter: nameof(UserAccess.UpdatedBy)),
                (Rule: await IsInvalidAsync(userAccess.CreatedDate), Parameter: nameof(UserAccess.CreatedDate)),
                (Rule: await IsInvalidAsync(userAccess.UpdatedDate), Parameter: nameof(UserAccess.UpdatedDate)));
        }

        private static void ValidateUserAccessIsNotNull(UserAccess userAccess)
        {
            if (userAccess is null)
            {
                throw new NullUserAccessException("User access is null.");
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
            var invalidUserAccessException =
                new InvalidUserAccessException(
                    message: "Invalid user access. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidUserAccessException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidUserAccessException.ThrowIfContainsErrors();
        }
    }
}
