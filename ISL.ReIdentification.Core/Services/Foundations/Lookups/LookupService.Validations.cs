using System;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;

namespace ISL.ReIdentification.Core.Services.Foundations.Lookups
{
    public partial class LookupService
    {
        private void ValidateLookupOnAdd(Lookup lookup)
        {
            ValidateLookupIsNotNull(lookup);

            Validate(
                (Rule: IsInvalid(lookup.Id), Parameter: nameof(Lookup.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(lookup.CreatedDate), Parameter: nameof(Lookup.CreatedDate)),
                (Rule: IsInvalid(lookup.CreatedBy), Parameter: nameof(Lookup.CreatedBy)),
                (Rule: IsInvalid(lookup.UpdatedDate), Parameter: nameof(Lookup.UpdatedDate)),
                (Rule: IsInvalid(lookup.UpdatedBy), Parameter: nameof(Lookup.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: lookup.UpdatedDate,
                    secondDate: lookup.CreatedDate,
                    secondDateName: nameof(Lookup.CreatedDate)),
                Parameter: nameof(Lookup.UpdatedDate)),

                (Rule: IsNotSame(
                    first: lookup.UpdatedBy,
                    second: lookup.CreatedBy,
                    secondName: nameof(Lookup.CreatedBy)),
                Parameter: nameof(Lookup.UpdatedBy)),

                (Rule: IsNotRecent(lookup.CreatedDate), Parameter: nameof(Lookup.CreatedDate)));
        }

        private void ValidateLookupOnModify(Lookup lookup)
        {
            ValidateLookupIsNotNull(lookup);

            Validate(
                (Rule: IsInvalid(lookup.Id), Parameter: nameof(Lookup.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(lookup.CreatedDate), Parameter: nameof(Lookup.CreatedDate)),
                (Rule: IsInvalid(lookup.CreatedBy), Parameter: nameof(Lookup.CreatedBy)),
                (Rule: IsInvalid(lookup.UpdatedDate), Parameter: nameof(Lookup.UpdatedDate)),
                (Rule: IsInvalid(lookup.UpdatedBy), Parameter: nameof(Lookup.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: lookup.UpdatedDate,
                    secondDate: lookup.CreatedDate,
                    secondDateName: nameof(Lookup.CreatedDate)),
                Parameter: nameof(Lookup.UpdatedDate)),

                (Rule: IsNotRecent(lookup.UpdatedDate), Parameter: nameof(lookup.UpdatedDate)));
        }

        public void ValidateLookupId(Guid lookupId) =>
            Validate((Rule: IsInvalid(lookupId), Parameter: nameof(Lookup.Id)));

        private static void ValidateStorageLookup(Lookup maybeLookup, Guid lookupId)
        {
            if (maybeLookup is null)
            {
                throw new NotFoundLookupException(lookupId);
            }
        }

        private static void ValidateLookupIsNotNull(Lookup lookup)
        {
            if (lookup is null)
            {
                throw new NullLookupException(message: "Lookup is null.");
            }
        }

        private static void ValidateAgainstStorageLookupOnModify(Lookup inputLookup, Lookup storageLookup)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputLookup.CreatedDate,
                    secondDate: storageLookup.CreatedDate,
                    secondDateName: nameof(Lookup.CreatedDate)),
                Parameter: nameof(Lookup.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputLookup.CreatedBy,
                    second: storageLookup.CreatedBy,
                    secondName: nameof(Lookup.CreatedBy)),
                Parameter: nameof(Lookup.CreatedBy)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsNotSame(
           string first,
           string second,
           string secondName) => new
           {
               Condition = first != second,
               Message = $"Text is not the same as {secondName}"
           };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
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