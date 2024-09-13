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
                Parameter: nameof(Lookup.UpdatedDate)));
        }

        private static void ValidateLookupIsNotNull(Lookup lookup)
        {
            if (lookup is null)
            {
                throw new NullLookupException(message: "Lookup is null.");
            }
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

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

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