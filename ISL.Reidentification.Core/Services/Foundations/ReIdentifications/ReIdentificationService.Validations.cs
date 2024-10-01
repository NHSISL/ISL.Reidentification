﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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

            Validate(
                (Rule: await IsInvalidAsync(identificationRequest.IdentificationItems),
                Parameter: nameof(IdentificationRequest.IdentificationItems)),

                (Rule: await IsInvalidAsync(identificationRequest.UserIdentifier),
                Parameter: nameof(IdentificationRequest.UserIdentifier)),

                (Rule: await IsInvalidAsync(identificationRequest.Purpose),
                Parameter: nameof(IdentificationRequest.Purpose)),

                (Rule: await IsInvalidAsync(identificationRequest.Organisation),
                Parameter: nameof(IdentificationRequest.Organisation)),

                (Rule: await IsInvalidAsync(identificationRequest.Reason),
                Parameter: nameof(IdentificationRequest.Reason)),

                (Rule: await IsNotUniqueAsync(identificationRequest.IdentificationItems),
                Parameter: nameof(IdentificationRequest.IdentificationItems)));
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

        private static async ValueTask<dynamic> IsInvalidAsync(List<IdentificationItem>? identificationItems) => new
        {
            Condition = identificationItems is null || identificationItems.Count == 0,
            Message = "IdentificationItems is invalid"
        };

        private static async ValueTask<dynamic> IsNotUniqueAsync(List<IdentificationItem>? identificationItems) => new
        {
            Condition = IsNotUniqueList(identificationItems),
            Message = "IdentificationItems.RowNumber is invalid.  There are duplicate RowNumbers."
        };

        private static bool IsNotUniqueList(List<IdentificationItem>? identificationItems)
        {
            return identificationItems is not null
                && identificationItems.Count >= 0
                && identificationItems.Select(item => item.RowNumber)
                    .Distinct().Count() != identificationItems.Count();
        }

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
