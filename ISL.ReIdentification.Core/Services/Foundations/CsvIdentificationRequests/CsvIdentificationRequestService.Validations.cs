// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.CsvIdentificationRequests;
using ISL.ReIdentification.Core.Models.Foundations.CsvIdentificationRequests.Exceptions;

namespace ISL.ReIdentification.Core.Services.Foundations.CsvIdentificationRequests
{
    public partial class CsvIdentificationRequestService
    {
        private async ValueTask ValidateCsvIdentificationRequestOnAdd(
            CsvIdentificationRequest csvIdentificationRequest)
        {
            ValidateCsvIdentificationRequestIsNotNull(csvIdentificationRequest);

            Validate(
                (Rule: await IsInvalidAsync(csvIdentificationRequest.Id),
                Parameter: nameof(CsvIdentificationRequest.Id)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.RequesterFirstName),
                Parameter: nameof(CsvIdentificationRequest.RequesterFirstName)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.RequesterLastName),
                Parameter: nameof(CsvIdentificationRequest.RequesterLastName)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.RequesterEmail),
                Parameter: nameof(CsvIdentificationRequest.RequesterEmail)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.RecipientFirstName),
                Parameter: nameof(CsvIdentificationRequest.RecipientFirstName)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.RecipientLastName),
                Parameter: nameof(CsvIdentificationRequest.RecipientLastName)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.RecipientEmail),
                Parameter: nameof(CsvIdentificationRequest.RecipientEmail)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.IdentifierColumn),
                Parameter: nameof(CsvIdentificationRequest.IdentifierColumn)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.CreatedBy),
                Parameter: nameof(CsvIdentificationRequest.CreatedBy)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.UpdatedBy),
                Parameter: nameof(CsvIdentificationRequest.UpdatedBy)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.CreatedDate),
                Parameter: nameof(CsvIdentificationRequest.CreatedDate)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.UpdatedDate),
                Parameter: nameof(CsvIdentificationRequest.UpdatedDate)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.RequesterFirstName, 255),
                Parameter: nameof(CsvIdentificationRequest.RequesterFirstName)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.RequesterLastName, 255),
                Parameter: nameof(CsvIdentificationRequest.RequesterLastName)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.RequesterEmail, 320),
                Parameter: nameof(CsvIdentificationRequest.RequesterEmail)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.RecipientFirstName, 255),
                Parameter: nameof(CsvIdentificationRequest.RecipientFirstName)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.RecipientLastName, 255),
                Parameter: nameof(CsvIdentificationRequest.RecipientLastName)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.RecipientEmail, 320),
                Parameter: nameof(CsvIdentificationRequest.RecipientEmail)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.IdentifierColumn, 10),
                Parameter: nameof(CsvIdentificationRequest.IdentifierColumn)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.CreatedBy, 255),
                Parameter: nameof(CsvIdentificationRequest.CreatedBy)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.UpdatedBy, 255),
                Parameter: nameof(CsvIdentificationRequest.UpdatedBy)),

                (Rule: await IsNotSameAsync(
                    first: csvIdentificationRequest.UpdatedBy,
                    second: csvIdentificationRequest.CreatedBy,
                    secondName: nameof(CsvIdentificationRequest.CreatedBy)),

                Parameter: nameof(CsvIdentificationRequest.UpdatedBy)),

                (Rule: await IsNotSameAsync(
                    first: csvIdentificationRequest.CreatedDate,
                    second: csvIdentificationRequest.UpdatedDate,
                    secondName: nameof(CsvIdentificationRequest.CreatedDate)),

                Parameter: nameof(CsvIdentificationRequest.UpdatedDate)),

                (Rule: await IsNotRecentAsync(csvIdentificationRequest.CreatedDate),
                Parameter: nameof(CsvIdentificationRequest.CreatedDate)));
        }

        private async ValueTask ValidateCsvIdentificationRequestOnModify(
            CsvIdentificationRequest csvIdentificationRequest)
        {
            ValidateCsvIdentificationRequestIsNotNull(csvIdentificationRequest);

            Validate(
                (Rule: await IsInvalidAsync(csvIdentificationRequest.Id),
                Parameter: nameof(CsvIdentificationRequest.Id)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.RequesterFirstName),
                Parameter: nameof(CsvIdentificationRequest.RequesterFirstName)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.RequesterLastName),
                Parameter: nameof(CsvIdentificationRequest.RequesterLastName)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.RequesterEmail),
                Parameter: nameof(CsvIdentificationRequest.RequesterEmail)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.RecipientFirstName),
                Parameter: nameof(CsvIdentificationRequest.RecipientFirstName)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.RecipientLastName),
                Parameter: nameof(CsvIdentificationRequest.RecipientLastName)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.RecipientEmail),
                Parameter: nameof(CsvIdentificationRequest.RecipientEmail)),

                (Rule: await IsInvalidAsync(
                    csvIdentificationRequest.IdentifierColumn),

                Parameter: nameof(CsvIdentificationRequest.IdentifierColumn)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.CreatedBy),
                Parameter: nameof(CsvIdentificationRequest.CreatedBy)),

                (Rule: await IsInvalidAsync(csvIdentificationRequest.UpdatedBy),
                Parameter: nameof(CsvIdentificationRequest.UpdatedBy)),

                (Rule: await IsInvalidAsync(
                    csvIdentificationRequest.CreatedDate),

                Parameter: nameof(CsvIdentificationRequest.CreatedDate)),

                (Rule: await IsInvalidAsync(
                    csvIdentificationRequest.UpdatedDate),

                Parameter: nameof(CsvIdentificationRequest.UpdatedDate)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.RequesterFirstName, 255),
                Parameter: nameof(CsvIdentificationRequest.RequesterFirstName)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.RequesterLastName, 255),
                Parameter: nameof(CsvIdentificationRequest.RequesterLastName)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.RequesterEmail, 320),
                Parameter: nameof(CsvIdentificationRequest.RequesterEmail)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.RecipientFirstName, 255),
                Parameter: nameof(CsvIdentificationRequest.RecipientFirstName)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.RecipientLastName, 255),
                Parameter: nameof(CsvIdentificationRequest.RecipientLastName)),

                (Rule: await IsInvalidLengthAsync(csvIdentificationRequest.RecipientEmail, 320),
                Parameter: nameof(CsvIdentificationRequest.RecipientEmail)),

                (Rule: await IsInvalidLengthAsync(
                    csvIdentificationRequest.IdentifierColumn, 10),

                Parameter: nameof(CsvIdentificationRequest.IdentifierColumn)),

                (Rule: await IsInvalidLengthAsync(
                    csvIdentificationRequest.CreatedBy, 255),

                Parameter: nameof(CsvIdentificationRequest.CreatedBy)),

                (Rule: await IsInvalidLengthAsync(
                    csvIdentificationRequest.UpdatedBy, 255),

                Parameter: nameof(CsvIdentificationRequest.UpdatedBy)),

                (Rule: await IsSameAsync(
                    firstDate: csvIdentificationRequest.UpdatedDate,
                    secondDate: csvIdentificationRequest.CreatedDate,
                    nameof(CsvIdentificationRequest.CreatedDate)),

                Parameter: nameof(CsvIdentificationRequest.UpdatedDate)),

                (Rule: await IsNotRecentAsync(csvIdentificationRequest.UpdatedDate),
                    Parameter: nameof(CsvIdentificationRequest.UpdatedDate)));
        }

        private static async ValueTask ValidateCsvIdentificationRequestIdAsync(Guid csvIdentificationRequestId)
        {
            Validate(
                (Rule: await IsInvalidAsync(csvIdentificationRequestId),
                Parameter: nameof(CsvIdentificationRequest.Id)));
        }

        private static void ValidateCsvIdentificationRequestIsNotNull(CsvIdentificationRequest csvIdentificationRequest)
        {
            if (csvIdentificationRequest is null)
            {
                throw new NullCsvIdentificationRequestException("Delegated access is null.");
            }
        }

        private static async ValueTask ValidateStorageCsvIdentificationRequestAsync(CsvIdentificationRequest maybeCsvIdentificationRequest,
            Guid id)
        {
            if (maybeCsvIdentificationRequest is null)
            {
                throw new NotFoundCsvIdentificationRequestException(
                    message: $"CsvIdentificationRequest not found with id: {id}");
            }
        }

        private static async ValueTask ValidateAgainstStorageCsvIdentificationRequestOnModifyAsync(
            CsvIdentificationRequest inputCsvIdentificationRequest, CsvIdentificationRequest storageCsvIdentificationRequest)
        {
            Validate(
                (Rule: await IsNotSameAsync(
                    first: inputCsvIdentificationRequest.CreatedBy,
                    second: storageCsvIdentificationRequest.CreatedBy,
                    secondName: nameof(CsvIdentificationRequest.CreatedBy)),

                Parameter: nameof(CsvIdentificationRequest.CreatedBy)),

                (Rule: await IsNotSameAsync(
                    first: inputCsvIdentificationRequest.CreatedDate,
                    second: storageCsvIdentificationRequest.CreatedDate,
                    secondName: nameof(CsvIdentificationRequest.CreatedDate)),

                Parameter: nameof(CsvIdentificationRequest.CreatedDate)),

                (Rule: await IsSameAsync(
                    firstDate: inputCsvIdentificationRequest.UpdatedDate,
                    secondDate: storageCsvIdentificationRequest.UpdatedDate,
                    secondDateName: nameof(CsvIdentificationRequest.UpdatedDate)),

                Parameter: nameof(CsvIdentificationRequest.UpdatedDate)));
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
            var invalidCsvIdentificationRequestException =
                new InvalidCsvIdentificationRequestException(
                    message: "Invalid delegated access. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidCsvIdentificationRequestException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidCsvIdentificationRequestException.ThrowIfContainsErrors();
        }
    }
}
