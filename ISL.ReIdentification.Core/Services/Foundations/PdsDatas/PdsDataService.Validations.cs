// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas.Exceptions;

namespace ISL.ReIdentification.Core.Services.Foundations.PdsDatas
{
    public partial class PdsDataService
    {
        public async ValueTask ValidatePdsDataId(Guid pdsDataId) =>
            Validate((Rule: await IsInvalidAsync(pdsDataId), Parameter: nameof(PdsData.Id)));

        private async static ValueTask ValidateStoragePdsData(PdsData maybePdsData, Guid pdsDataId)
        {
            if (maybePdsData is null)
            {
                throw new NotFoundPdsDataException(message: $"PDS data not found with Id: {pdsDataId}");
            }
        }

        private static async ValueTask<dynamic> IsInvalidAsync(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is invalid"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidPdsDataException =
                new InvalidPdsDataException(
                    message: "Invalid pds data. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPdsDataException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPdsDataException.ThrowIfContainsErrors();
        }
    }
}
