// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;

namespace ISL.ReIdentification.Core.Services.Foundations.Lookups
{
    public partial class LookupService
    {
        private async ValueTask ValidateLookupOnAdd(Lookup lookup)
        {
            await ValidateLookupIsNotNull(lookup);

            Validate(
                (Rule: await IsInvalidAsync(lookup.Id), Parameter: nameof(Lookup.Id)),
                (Rule: await IsInvalidAsync(lookup.Name), Parameter: nameof(Lookup.Name)),
                (Rule: await IsInvalidAsync(lookup.CreatedDate), Parameter: nameof(Lookup.CreatedDate)),
                (Rule: await IsInvalidAsync(lookup.CreatedBy), Parameter: nameof(Lookup.CreatedBy)),
                (Rule: await IsInvalidAsync(lookup.UpdatedDate), Parameter: nameof(Lookup.UpdatedDate)),
                (Rule: await IsInvalidAsync(lookup.UpdatedBy), Parameter: nameof(Lookup.UpdatedBy)),
                (Rule: await IsInvalidLengthAsync(lookup.Name, 450), Parameter: nameof(Lookup.Name)),
                (Rule: await IsInvalidLengthAsync(lookup.CreatedBy, 255), Parameter: nameof(Lookup.CreatedBy)),
                (Rule: await IsInvalidLengthAsync(lookup.UpdatedBy, 255), Parameter: nameof(Lookup.UpdatedBy)),

                (Rule: await IsNotSameAsync(
                    first: lookup.UpdatedBy,
                    second: lookup.CreatedBy,
                    secondName: nameof(Lookup.CreatedBy)),

                Parameter: nameof(Lookup.UpdatedBy)),

                (Rule: await IsNotSameAsync(
                    first: lookup.UpdatedDate,
                    second: lookup.CreatedDate,
                    secondName: nameof(Lookup.CreatedDate)),

                Parameter: nameof(Lookup.UpdatedDate)),

                (Rule: await IsNotRecentAsync(lookup.CreatedDate), Parameter: nameof(Lookup.CreatedDate)));
        }

        private async ValueTask ValidateLookupOnModify(Lookup lookup)
        {
            await ValidateLookupIsNotNull(lookup);

            Validate(
                (Rule: await IsInvalidAsync(lookup.Id), Parameter: nameof(Lookup.Id)),
                (Rule: await IsInvalidAsync(lookup.Name), Parameter: nameof(Lookup.Name)),
                (Rule: await IsInvalidAsync(lookup.CreatedDate), Parameter: nameof(Lookup.CreatedDate)),
                (Rule: await IsInvalidAsync(lookup.CreatedBy), Parameter: nameof(Lookup.CreatedBy)),
                (Rule: await IsInvalidAsync(lookup.UpdatedDate), Parameter: nameof(Lookup.UpdatedDate)),
                (Rule: await IsInvalidAsync(lookup.UpdatedBy), Parameter: nameof(Lookup.UpdatedBy)),
                (Rule: await IsInvalidLengthAsync(lookup.Name, 450), Parameter: nameof(Lookup.Name)),
                (Rule: await IsInvalidLengthAsync(lookup.CreatedBy, 255), Parameter: nameof(Lookup.CreatedBy)),
                (Rule: await IsInvalidLengthAsync(lookup.UpdatedBy, 255), Parameter: nameof(Lookup.UpdatedBy)),

                (Rule: await IsSameAsAsync(
                    firstDate: lookup.UpdatedDate,
                    secondDate: lookup.CreatedDate,
                    secondDateName: nameof(Lookup.CreatedDate)),
                Parameter: nameof(Lookup.UpdatedDate)),

                (Rule: await IsNotRecentAsync(lookup.UpdatedDate), Parameter: nameof(lookup.UpdatedDate)));
        }

        public async ValueTask ValidateLookupId(Guid lookupId) =>
            Validate((Rule: await IsInvalidAsync(lookupId), Parameter: nameof(Lookup.Id)));

        private async static ValueTask ValidateStorageLookup(Lookup maybeLookup, Guid lookupId)
        {
            if (maybeLookup is null)
            {
                throw new NotFoundLookupException(lookupId);
            }
        }

        private async static ValueTask ValidateLookupIsNotNull(Lookup lookup)
        {
            if (lookup is null)
            {
                throw new NullLookupException(message: "Lookup is null.");
            }
        }

        private async ValueTask ValidateAgainstStorageLookupOnModify(Lookup inputLookup, Lookup storageLookup)
        {
            Validate(
                (Rule: await IsNotSameAsync(
                    first: inputLookup.CreatedBy,
                    second: storageLookup.CreatedBy,
                    secondName: nameof(Lookup.CreatedBy)),
                Parameter: nameof(Lookup.CreatedBy)),

                (Rule: await IsNotSameAsync(
                    first: inputLookup.CreatedDate,
                    second: storageLookup.CreatedDate,
                    secondName: nameof(Lookup.CreatedDate)),
                Parameter: nameof(Lookup.CreatedDate)),

                (Rule: await IsSameAsAsync(
                    firstDate: inputLookup.UpdatedDate,
                    secondDate: storageLookup.UpdatedDate,
                    secondDateName: nameof(Lookup.UpdatedDate)),
                Parameter: nameof(Lookup.UpdatedDate)));
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

        private static async ValueTask<dynamic> IsInvalidLengthAsync(string text, int maxLength) => new
        {
            Condition = await IsExceedingLengthAsync(text, maxLength),
            Message = $"Text exceed max length of {maxLength} characters"
        };

        private static async ValueTask<bool> IsExceedingLengthAsync(string text, int maxLength) =>
            (text ?? string.Empty).Length > maxLength;

        private static async ValueTask<dynamic> IsInvalidAsync(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is invalid"
        };

        private static async ValueTask<dynamic> IsSameAsAsync(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static async ValueTask<dynamic> IsNotSameAsync(
            string first,
            string second,
            string secondName) => new
            {
                Condition = first != second,
                Message = $"Text is not the same as {secondName}"
            };

        private static async ValueTask<dynamic> IsNotSameAsync(
            DateTimeOffset first,
            DateTimeOffset second,
            string secondName) => new
            {
                Condition = first != second,
                Message = $"Date is not the same as {secondName}"
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
            var invalidLookupException =
                new InvalidLookupException(
                    message: "Invalid lookup. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidLookupException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidLookupException.ThrowIfContainsErrors();
        }
    }
}