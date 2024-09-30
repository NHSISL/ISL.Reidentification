// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications.Exceptions;

namespace ISL.ReIdentification.Core.Services.Foundations.ReIdentifications
{
    public partial class ReIdentificationService
    {
        private async ValueTask ValidateIdentificationRequestOnProcessAsync(IdentificationRequest identificationRequest)
        {
            ValidateIdentificationRequestIsNotNull(identificationRequest);

            //TODO: Update validation logic
            //Validate(

            //    (Rule: await IsInvalidAsync(identificationRequest.Identifier),
            //    Parameter: nameof(IdentificationRequest.Identifier)),

            //    (Rule: await IsInvalidAsync(identificationRequest.UserEmail),
            //    Parameter: nameof(IdentificationRequest.UserEmail)),

            //    (Rule: await IsInvalidLengthAsync(identificationRequest.UserEmail, 320),
            //    Parameter: nameof(IdentificationRequest.UserEmail)),

            //    (Rule: await IsInvalidLengthAsync(identificationRequest.Identifier, 10),
            //    Parameter: nameof(IdentificationRequest.Identifier)));
        }

        private static void ValidateIdentificationRequestIsNotNull(IdentificationRequest identificationRequest)
        {
            if (identificationRequest is null)
            {
                throw new NullIdentificationRequestException("Identification request is null.");
            }
        }

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


        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidIdentificationRequestException =
                new InvalidIdentificationRequestException(
                    message: "Invalid identification request. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidIdentificationRequestException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidIdentificationRequestException.ThrowIfContainsErrors();
        }
    }
}
