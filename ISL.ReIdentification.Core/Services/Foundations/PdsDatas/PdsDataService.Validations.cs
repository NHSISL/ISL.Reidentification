// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas.Exceptions;

namespace ISL.ReIdentification.Core.Services.Foundations.PdsDatas
{
    public partial class PdsDataService
    {
        public async ValueTask ValidatePdsDataRowId(long pdsDataRowId) =>
            Validate((Rule: await IsInvalidAsync(pdsDataRowId), Parameter: nameof(PdsData.RowId)));

        private async static ValueTask ValidateStoragePdsData(PdsData maybePdsData, long pdsDataRowId)
        {
            if (maybePdsData is null)
            {
                throw new NotFoundPdsDataException(message: $"PDS data not found with Id: {pdsDataRowId}");
            }
        }

        private static async ValueTask<dynamic> IsInvalidAsync(long id) => new
        {
            Condition = id == default(long),
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
