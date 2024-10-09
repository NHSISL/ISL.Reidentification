// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts;
using ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions;

namespace ISL.ReIdentification.Core.Services.Foundations.ImpersonationContexts
{
    public partial class ImpersonationContextService
    {
        private async ValueTask ValidateImpersonationContextOnAdd(ImpersonationContext impersonationContext)
        {
            ValidateImpersonationContextIsNotNull(impersonationContext);

            Validate(
                (Rule: await IsInvalidAsync(impersonationContext.Id), Parameter: nameof(ImpersonationContext.Id)),

                (Rule: await IsInvalidAsync(impersonationContext.RequesterFirstName),
                Parameter: nameof(ImpersonationContext.RequesterFirstName)),

                (Rule: await IsInvalidAsync(impersonationContext.RequesterLastName),
                Parameter: nameof(ImpersonationContext.RequesterLastName)),

                (Rule: await IsInvalidAsync(impersonationContext.RequesterEmail),
                Parameter: nameof(ImpersonationContext.RequesterEmail)),

                (Rule: await IsInvalidAsync(impersonationContext.RecipientFirstName),
                Parameter: nameof(ImpersonationContext.RecipientFirstName)),

                (Rule: await IsInvalidAsync(impersonationContext.RecipientLastName),
                Parameter: nameof(ImpersonationContext.RecipientLastName)),

                (Rule: await IsInvalidAsync(impersonationContext.RecipientEmail),
                Parameter: nameof(ImpersonationContext.RecipientEmail)),

                (Rule: await IsInvalidAsync(impersonationContext.IdentifierColumn),
                Parameter: nameof(ImpersonationContext.IdentifierColumn)),

                (Rule: await IsInvalidAsync(impersonationContext.CreatedBy),
                Parameter: nameof(ImpersonationContext.CreatedBy)),

                (Rule: await IsInvalidAsync(impersonationContext.UpdatedBy),
                Parameter: nameof(ImpersonationContext.UpdatedBy)),

                (Rule: await IsInvalidAsync(impersonationContext.CreatedDate),
                Parameter: nameof(ImpersonationContext.CreatedDate)),

                (Rule: await IsInvalidAsync(impersonationContext.UpdatedDate),
                Parameter: nameof(ImpersonationContext.UpdatedDate)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.RequesterFirstName, 255),
                Parameter: nameof(ImpersonationContext.RequesterFirstName)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.RequesterLastName, 255),
                Parameter: nameof(ImpersonationContext.RequesterLastName)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.RequesterEmail, 320),
                Parameter: nameof(ImpersonationContext.RequesterEmail)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.RecipientFirstName, 255),
                Parameter: nameof(ImpersonationContext.RecipientFirstName)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.RecipientLastName, 255),
                Parameter: nameof(ImpersonationContext.RecipientLastName)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.RecipientEmail, 320),
                Parameter: nameof(ImpersonationContext.RecipientEmail)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.IdentifierColumn, 10),
                Parameter: nameof(ImpersonationContext.IdentifierColumn)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.CreatedBy, 255),
                Parameter: nameof(ImpersonationContext.CreatedBy)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.UpdatedBy, 255),
                Parameter: nameof(ImpersonationContext.UpdatedBy)),

                (Rule: await IsNotSameAsync(
                    first: impersonationContext.UpdatedBy,
                    second: impersonationContext.CreatedBy,
                    secondName: nameof(ImpersonationContext.CreatedBy)),

                Parameter: nameof(ImpersonationContext.UpdatedBy)),

                (Rule: await IsNotSameAsync(
                    first: impersonationContext.CreatedDate,
                    second: impersonationContext.UpdatedDate,
                    secondName: nameof(ImpersonationContext.CreatedDate)),

                Parameter: nameof(ImpersonationContext.UpdatedDate)),

                (Rule: await IsNotRecentAsync(impersonationContext.CreatedDate),
                Parameter: nameof(ImpersonationContext.CreatedDate)));
        }

        private async ValueTask ValidateImpersonationContextOnModify(ImpersonationContext impersonationContext)
        {
            ValidateImpersonationContextIsNotNull(impersonationContext);

            Validate(
                (Rule: await IsInvalidAsync(impersonationContext.Id), Parameter: nameof(ImpersonationContext.Id)),

                (Rule: await IsInvalidAsync(impersonationContext.RequesterFirstName),
                Parameter: nameof(ImpersonationContext.RequesterFirstName)),

                (Rule: await IsInvalidAsync(impersonationContext.RequesterLastName),
                Parameter: nameof(ImpersonationContext.RequesterLastName)),

                (Rule: await IsInvalidAsync(impersonationContext.RequesterEmail),
                Parameter: nameof(ImpersonationContext.RequesterEmail)),

                (Rule: await IsInvalidAsync(impersonationContext.RecipientFirstName),
                Parameter: nameof(ImpersonationContext.RecipientFirstName)),

                (Rule: await IsInvalidAsync(impersonationContext.RecipientLastName),
                Parameter: nameof(ImpersonationContext.RecipientLastName)),

                (Rule: await IsInvalidAsync(impersonationContext.RecipientEmail),
                Parameter: nameof(ImpersonationContext.RecipientEmail)),

                (Rule: await IsInvalidAsync(
                    impersonationContext.IdentifierColumn),

                Parameter: nameof(ImpersonationContext.IdentifierColumn)),

                (Rule: await IsInvalidAsync(impersonationContext.CreatedBy),
                Parameter: nameof(ImpersonationContext.CreatedBy)),

                (Rule: await IsInvalidAsync(impersonationContext.UpdatedBy),
                Parameter: nameof(ImpersonationContext.UpdatedBy)),

                (Rule: await IsInvalidAsync(
                    impersonationContext.CreatedDate),

                Parameter: nameof(ImpersonationContext.CreatedDate)),

                (Rule: await IsInvalidAsync(
                    impersonationContext.UpdatedDate),

                Parameter: nameof(ImpersonationContext.UpdatedDate)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.RequesterFirstName, 255),
                Parameter: nameof(ImpersonationContext.RequesterFirstName)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.RequesterLastName, 255),
                Parameter: nameof(ImpersonationContext.RequesterLastName)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.RequesterEmail, 320),
                Parameter: nameof(ImpersonationContext.RequesterEmail)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.RecipientFirstName, 255),
                Parameter: nameof(ImpersonationContext.RecipientFirstName)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.RecipientLastName, 255),
                Parameter: nameof(ImpersonationContext.RecipientLastName)),

                (Rule: await IsInvalidLengthAsync(impersonationContext.RecipientEmail, 320),
                Parameter: nameof(ImpersonationContext.RecipientEmail)),

                (Rule: await IsInvalidLengthAsync(
                    impersonationContext.IdentifierColumn, 10),

                Parameter: nameof(ImpersonationContext.IdentifierColumn)),

                (Rule: await IsInvalidLengthAsync(
                    impersonationContext.CreatedBy, 255),

                Parameter: nameof(ImpersonationContext.CreatedBy)),

                (Rule: await IsInvalidLengthAsync(
                    impersonationContext.UpdatedBy, 255),

                Parameter: nameof(ImpersonationContext.UpdatedBy)),

                (Rule: await IsSameAsync(
                    firstDate: impersonationContext.UpdatedDate,
                    secondDate: impersonationContext.CreatedDate,
                    nameof(ImpersonationContext.CreatedDate)),

                Parameter: nameof(ImpersonationContext.UpdatedDate)),

                (Rule: await IsNotRecentAsync(impersonationContext.UpdatedDate),
                    Parameter: nameof(ImpersonationContext.UpdatedDate)));
        }

        private static async ValueTask ValidateImpersonationContextIdAsync(Guid impersonationContextId) =>
            Validate((Rule: await IsInvalidAsync(impersonationContextId), Parameter: nameof(ImpersonationContext.Id)));

        private static void ValidateImpersonationContextIsNotNull(ImpersonationContext impersonationContext)
        {
            if (impersonationContext is null)
            {
                throw new NullImpersonationContextException("Impersonation context is null.");
            }
        }

        private static async ValueTask ValidateStorageImpersonationContextAsync(ImpersonationContext maybeImpersonationContext,
            Guid id)
        {
            if (maybeImpersonationContext is null)
            {
                throw new NotFoundImpersonationContextException(
                    message: $"ImpersonationContext not found with id: {id}");
            }
        }

        private static async ValueTask ValidateAgainstStorageImpersonationContextOnModifyAsync(
            ImpersonationContext inputImpersonationContext, ImpersonationContext storageImpersonationContext)
        {
            Validate(
                (Rule: await IsNotSameAsync(
                    first: inputImpersonationContext.CreatedBy,
                    second: storageImpersonationContext.CreatedBy,
                    secondName: nameof(ImpersonationContext.CreatedBy)),

                Parameter: nameof(ImpersonationContext.CreatedBy)),

                (Rule: await IsNotSameAsync(
                    first: inputImpersonationContext.CreatedDate,
                    second: storageImpersonationContext.CreatedDate,
                    secondName: nameof(ImpersonationContext.CreatedDate)),

                Parameter: nameof(ImpersonationContext.CreatedDate)),

                (Rule: await IsSameAsync(
                    firstDate: inputImpersonationContext.UpdatedDate,
                    secondDate: storageImpersonationContext.UpdatedDate,
                    secondDateName: nameof(ImpersonationContext.UpdatedDate)),

                Parameter: nameof(ImpersonationContext.UpdatedDate)));
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

        private static async ValueTask<dynamic> IsInvalidLengthAsync(string text, int maxLength) => new
        {
            Condition = await IsExceedingLengthAsync(text, maxLength),
            Message = $"Text exceed max length of {maxLength} characters"
        };

        private static async ValueTask<bool> IsExceedingLengthAsync(string text, int maxLength) =>
            (text ?? string.Empty).Length > maxLength;

        private static async ValueTask<dynamic> IsSameAsync(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static async ValueTask<dynamic> IsNotSameAsync(
            DateTimeOffset first,
            DateTimeOffset second,
            string secondName) => new
            {
                Condition = first != second,
                Message = $"Date is not the same as {secondName}"
            };

        private static async ValueTask<dynamic> IsNotSameAsync(
            string first,
            string second,
            string secondName) => new
            {
                Condition = first != second,
                Message = $"Text is not the same as {secondName}"
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
            var invalidImpersonationContextException =
                new InvalidImpersonationContextException(
                    message: "Invalid impersonation context. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidImpersonationContextException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidImpersonationContextException.ThrowIfContainsErrors();
        }
    }
}
