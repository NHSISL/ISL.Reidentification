﻿// ---------------------------------------------------------
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
                (Rule: await IsInvalidAsync(userAccess.UpdatedDate), Parameter: nameof(UserAccess.UpdatedDate)),

                (Rule: await IsNotSameAsync(
                    createBy: userAccess.UpdatedBy,
                    updatedBy: userAccess.CreatedBy,
                    createdByName: nameof(UserAccess.CreatedBy)),

                Parameter: nameof(UserAccess.UpdatedBy)),

                (Rule: await IsNotSameAsync(
                    createdDate: userAccess.CreatedDate,
                    updatedDate: userAccess.UpdatedDate,
                    nameof(UserAccess.CreatedDate)),

                Parameter: nameof(UserAccess.UpdatedDate)),

                (Rule: await IsNotRecentAsync(userAccess.CreatedDate), Parameter: nameof(UserAccess.CreatedDate)));
        }

        private async ValueTask ValidateUserAccessOnModifyAsync(UserAccess userAccess)
        {
            ValidateUserAccessIsNotNull(userAccess);
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

        private static async ValueTask<dynamic> IsNotSameAsync(
            DateTimeOffset createdDate,
            DateTimeOffset updatedDate,
            string createdDateName) => new
            {
                Condition = createdDate != updatedDate,
                Message = $"Date is not the same as {createdDateName}"
            };

        private static async ValueTask<dynamic> IsNotSameAsync(
            string createBy,
            string updatedBy,
            string createdByName) => new
            {
                Condition = createBy != updatedBy,
                Message = $"Text is not the same as {createdByName}"
            };

        private async ValueTask<dynamic> IsNotRecentAsync(DateTimeOffset date)
        {
            var (isNotRecent, startDate, endDate) = await IsDateNotRecentAsync(date);

            return new
            {
                Condition = isNotRecent,
                Message = $"Date is not recent. Expected a value between {startDate} and {endDate} but found {date}"
            };
        }

        private async ValueTask<(bool IsNotRecent, DateTimeOffset StartDate, DateTimeOffset EndDate)>
            IsDateNotRecentAsync(DateTimeOffset date)
        {
            int pastSeconds = 60;
            int futureSeconds = 0;
            DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            if (currentDateTime == default)
            {
                return (false, default, default);
            }

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            DateTimeOffset startDate = currentDateTime.AddSeconds(-pastSeconds);
            DateTimeOffset endDate = currentDateTime.AddSeconds(futureSeconds);
            bool isNotRecent = timeDifference.TotalSeconds is > 60 or < 0;

            return (isNotRecent, startDate, endDate);
        }

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
